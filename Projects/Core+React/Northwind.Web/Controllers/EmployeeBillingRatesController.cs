using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NLog;
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using Northwind.Core.Interfaces.ContractRefactor;
using Northwind.Web.Helpers;
using Northwind.Web.Infrastructure.AuditLog;
using Northwind.Web.Infrastructure.Authorization;
using Northwind.Web.Infrastructure.Helpers;
using Northwind.Web.Models.ViewModels;
using static Northwind.Core.Entities.EnumGlobal;

namespace Northwind.Web.Controllers
{
    public class EmployeeBillingRatesController : Controller
    {
        private readonly IFileService _fileService;
        private readonly IConfiguration _configuration;
        private readonly IEmployeeBillingRatesService _employeeBillingRatesService;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IContractsService _contractRefactorService;
        private readonly Logger _logger;
        private readonly IContractResourceFileService _contractResourceFileService;
        string documentRoot;
        string filePath;
        public EmployeeBillingRatesController(
            IFileService fileService,
            IConfiguration configuration,
            IEmployeeBillingRatesService employeeBillingRatesService,
            IContractsService contractRefactorService,
            IContractResourceFileService contractResourceFileService,
            IUserService userService,
            IMapper mapper
            )
        {
            _fileService = fileService;
            _configuration = configuration;
            _employeeBillingRatesService = employeeBillingRatesService;
            _contractRefactorService = contractRefactorService;
            _contractResourceFileService = contractResourceFileService;
            _mapper = mapper;
            _userService = userService;
            documentRoot = configuration.GetSection("Document").GetValue<string>("DocumentRoot");
            _logger = LogManager.GetCurrentClassLogger();
        }

        [HttpGet]
        [Secure(ResourceType.EmployeeBillingRates, ResourceActionPermission.Add)]
        public ActionResult Add(Guid id)
        {
            try
            {
                var employeeBillingRates = new EmployeeBillingRatesViewModel();
                Guid billingRateGuid = Guid.NewGuid();
                var files = _contractRefactorService.GetFileByResourceGuid(id, Core.Entities.EnumGlobal.ResourceType.WorkBreakDownStructure.ToString());
                if (files != null)
                {
                    if (files.IsFile)
                        billingRateGuid = files.ContentResourceGuid;
                }
                employeeBillingRates.BillingRateGuid = billingRateGuid;
                employeeBillingRates.ContractGuid = id;
                return PartialView(employeeBillingRates);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(ex.ToString(), ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPost]
        [Secure(ResourceType.EmployeeBillingRates, ResourceActionPermission.Add)]
        public IActionResult Add([FromForm]EmployeeBillingRatesViewModel employeeBillingRates)
        {
            try
            {
                if (employeeBillingRates.FileToUpload == null)
                {
                    ModelState.AddModelError("", "Please insert the file.");
                    return BadRequest(ModelState);
                }

                var isfileValid = _fileService.UploadFileTypeCheck(employeeBillingRates.FileToUpload);
                employeeBillingRates.CreatedOn = CurrentDateTimeHelper.GetCurrentDateTime();
                employeeBillingRates.UpdatedOn = CurrentDateTimeHelper.GetCurrentDateTime();
                employeeBillingRates.CreatedBy = UserHelper.CurrentUserGuid(HttpContext);
                employeeBillingRates.UpdatedBy = UserHelper.CurrentUserGuid(HttpContext);
                employeeBillingRates.IsActive = true;
                employeeBillingRates.IsDeleted = false;
                var contractNumber = _contractRefactorService.GetContractNumberById(employeeBillingRates.ContractGuid);
                var contractResourceFile = _contractResourceFileService.GetFilePathByResourceIdAndKeys(Core.Entities.EnumGlobal.ResourceType.EmployeeBillingRates.ToString(), employeeBillingRates.ContractGuid);
                if (contractResourceFile != null && (employeeBillingRates.FileToUpload != null || employeeBillingRates.FileToUpload.Length != 0))
                {
                    var filename = "";
                    if (!isfileValid)
                    {
                        var directoryPath = Path.GetDirectoryName(contractResourceFile.FilePath);
                        filename = _fileService.FilePost($@"{documentRoot}/{contractResourceFile.FilePath}/", employeeBillingRates.FileToUpload);
                        employeeBillingRates.IsCsv = false;
                        filePath = $"{contractResourceFile.FilePath}/{filename}";
                    }
                    else
                    {
                        var files = _contractRefactorService.GetFileByResourceGuid(employeeBillingRates.ContractGuid, Core.Entities.EnumGlobal.ResourceType.EmployeeBillingRates.ToString());
                        var relativePath = $@"{documentRoot}/{contractResourceFile.FilePath}/";
                        var previousFile = string.Empty;
                        if (files != null)
                        {
                            previousFile = files.UploadFileName;
                        }
                        filename = _fileService.MoveFile(relativePath, previousFile, employeeBillingRates.FileToUpload);
                        employeeBillingRates.IsCsv = true;
                        var fullPath = $@"{relativePath}/{filename}";
                        Helpers.CsvValidationHelper.ChecksValidHeaderAndReadTheFile(fullPath, relativePath, previousFile, (Models.ViewModels.EnumGlobal.UploadMethodName)UploadMethodName.EmployeeBillingRate);
                        filePath = $"{contractResourceFile.FilePath}/{filename}";
                    }
                    employeeBillingRates.FilePath = filePath;
                    employeeBillingRates.UploadFileName = filename;
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var contractFile = ContractsMapper.MapEmployeeBillingRatesViewModelToContractFiles(employeeBillingRates);
                contractFile.ResourceType = Core.Entities.EnumGlobal.ResourceType.Contract.ToString();
                if (contractResourceFile != null)
                    contractFile.ParentId = contractResourceFile.ContractResourceFileGuid;
                _contractRefactorService.CheckAndInsertContractFile(contractFile);

                //audit log..
                var contractEntity = _contractRefactorService.GetContractEntityByContractId(contractFile.ResourceGuid);
                var additionalInformation = string.Format("{0} {1} the {2}", User.FindFirst("fullName").Value, CrudTypeForAdditionalLogMessage.Uploaded.ToString(), "Employee Billing Rates File");
                string additionalInformationURl = string.Empty;
                string resource = string.Empty;

                if (contractEntity.ParentContractGuid == Guid.Empty || contractEntity.ParentContractGuid == null)
                {
                    additionalInformationURl = _configuration.GetSection("SiteUrl").Value + ("/Contract/Details/" + contractEntity.ContractGuid);
                    resource = string.Format("{0} </br> Contract No:{1} </br> Project No:{2}  </br> File Name:{3}", "Employee Billing Rates", contractEntity.ContractNumber, contractEntity.ProjectNumber, employeeBillingRates.UploadFileName);
                }
                else
                {
                    additionalInformationURl = _configuration.GetSection("SiteUrl").Value + ("/Project/Details/" + contractEntity.ContractGuid);
                    resource = string.Format("{0} </br> TaskOrder No:{1} </br> Project No:{2}  </br> File Name:{3}", "Employee Billing Rates", contractEntity.ContractNumber, contractEntity.ProjectNumber, employeeBillingRates.UploadFileName);
                }

                AuditLogHandler.InfoLog(_logger, User.FindFirst("fullName").Value, UserHelper.CurrentUserGuid(HttpContext), contractFile, resource, contractFile.ContractResourceFileGuid, UserHelper.GetHostedIp(HttpContext), "File Uploaded", Guid.Empty, "Successful", "", additionalInformation, additionalInformationURl);
                //end of audit log.

                return Ok(new { status = ResponseStatus.success.ToString(), message = "Successfully Added !!" });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(ex.ToString(), ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpGet]
        [Secure(ResourceType.EmployeeBillingRates, ResourceActionPermission.Details)]
        public ActionResult Detail(Guid id)
        {
            //var employeeBillingRatesEntity = _employeeBillingRatesService.GetEmployeeBillingRatesById(id);
            //var model = _mapper.Map<EmployeeBillingRatesViewModel>(employeeBillingRatesEntity);
            try
            {
                var file = _contractRefactorService.GetFileByResourceGuid(id, Core.Entities.EnumGlobal.ResourceType.EmployeeBillingRates.ToString());
                var model = ContractsMapper.MapContractFilesToEmployeeBillingRatesViewModel(file);
                var user = _userService.GetUserByUserGuid(file.UpdatedBy);
                model.UpdatedByDisplayname = user.DisplayName;
                return PartialView(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(ex.ToString(), ex.Message);
                return BadRequest(ModelState);
            }
        }

        [Secure(ResourceType.EmployeeBillingRates, ResourceActionPermission.Details)]
        public IActionResult Get(Guid id)
        {
            try
            {
                //var model = _employeeBillingRatesService.GetEmployeeBillingRatesById(id);

                var file = _contractRefactorService.GetFileByResourceGuid(id, Core.Entities.EnumGlobal.ResourceType.EmployeeBillingRates.ToString());

                if (file != null)
                {
                    var model = ContractsMapper.MapContractFilesToEmployeeBillingRatesViewModel(file);
                    var filePath = $"{documentRoot}/{model.FilePath}";
                    var contractViewModels = CsvValidationHelper.ReadFile(filePath, (Models.ViewModels.EnumGlobal.UploadMethodName)UploadMethodName.EmployeeBillingRate);

                    var data = contractViewModels.Select(x => new
                    {
                        ContractGuid = id,
                        LaborCode = x.LaborCode,
                        EmployeeName = x.EmployeeName,
                        Rate = x.Rate,
                        StartDate = x.StartDate,
                        EndDate = x.EndDate
                    }).ToList();
                    return Json(new { data = data });
                }
                return Json(new { data = new List<dynamic>() });

            }
            catch (Exception ex)
            {
                ModelState.AddModelError(ex.ToString(), ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPost]
        [Secure(ResourceType.EmployeeBillingRates, ResourceActionPermission.Details)]
        public IActionResult Get([FromBody] string searchText)
        {
            try
            {
                var list = JsonConvert.DeserializeObject<List<GridHeaderModel>>(searchText);
                if (list.Count == 0)
                    return BadRequest(new { status = ResponseStatus.error.ToString(), path = " : No Any Data In The Row" });
                var contractGuid = list[0].ContractGuid;
                var model = _contractRefactorService.GetFileByResourceGuid(contractGuid, Core.Entities.EnumGlobal.ResourceType.EmployeeBillingRates.ToString());
                string fileName = string.Format(model.UploadFileName, System.AppContext.BaseDirectory);
                var contractResourceFile = _contractResourceFileService.GetFilePathByResourceIdAndKeys(Core.Entities.EnumGlobal.ResourceType.EmployeeBillingRates.ToString(), contractGuid);
                var listdata = list.Select(x => new
                {
                    LaborCode = x.LaborCode,
                    EmployeeName = x.EmployeeName,
                    Rate = x.Rate,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate
                }).ToList();

                var dynamicList = listdata.Cast<dynamic>().ToList();

                // saves the files to the folder specified 
                var relativePath = $@"{documentRoot}/{contractResourceFile.FilePath}/";
                var fullPath = $@"{relativePath}{fileName}";
                CsvValidationHelper.MoveFile(relativePath, fileName);
                CsvValidationHelper.SaveTheUpdatedCsv(dynamicList, fullPath);
                var previousFile = string.Empty;
                if (model != null)
                {
                    previousFile = model.UploadFileName;
                }
                // after file save the saved file is valid if not valid the file is deleted 
                Helpers.CsvValidationHelper.ChecksValidHeaderAndReadTheFile(fullPath, relativePath, previousFile, (Models.ViewModels.EnumGlobal.UploadMethodName)UploadMethodName.EmployeeBillingRate);

                //updates the new filename/path to the database\
                model.FilePath = contractResourceFile.FilePath + "/" + fileName; ;
                model.UploadFileName = fileName;
                model.IsCsv = true;
                model.IsFile = true;
                _contractRefactorService.CheckAndInsertContractFile(model);

                return Ok(new { status = ResponseStatus.success.ToString(), id = model.ContractResourceFileGuid });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(ex.ToString(), ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpGet]
        [Secure(ResourceType.EmployeeBillingRates, ResourceActionPermission.Edit)]
        public ActionResult Edit(Guid id)
        {
            try
            {
                var file = _contractRefactorService.GetFileByResourceGuid(id, Core.Entities.EnumGlobal.ResourceType.EmployeeBillingRates.ToString());
                var model = ContractsMapper.MapContractFilesToEmployeeBillingRatesViewModel(file);
                var user = _userService.GetUserByUserGuid(file.UpdatedBy);
                model.UpdatedByDisplayname = user.DisplayName;
                return PartialView(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(ex.ToString(), ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPost]
        [Secure(ResourceType.EmployeeBillingRates, ResourceActionPermission.Edit)]
        public IActionResult Edit([FromForm]EmployeeBillingRatesViewModel employeeBillingRates)
        {
            try
            {
                employeeBillingRates.UpdatedOn = CurrentDateTimeHelper.GetCurrentDateTime();
                employeeBillingRates.UpdatedBy = UserHelper.CurrentUserGuid(HttpContext);
                employeeBillingRates.IsActive = true;
                employeeBillingRates.IsDeleted = false;
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                //var employeeBillingRatesEntity = _mapper.Map<EmployeeBillingRates>(employeeBillingRates);
                //_employeeBillingRatesService.UpdateEmployeeBillingRates(employeeBillingRatesEntity);

                //later added
                var file = ContractsMapper.MapEmployeeBillingRatesViewModelToContractFiles(employeeBillingRates);
                _contractRefactorService.UpdateContractFile(file);

                return Ok(new { status = ResponseStatus.success.ToString(), message = "Successfully Edited !!" });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(ex.ToString(), ex.Message);
                return BadRequest(ModelState);
            }
        }
    }
}