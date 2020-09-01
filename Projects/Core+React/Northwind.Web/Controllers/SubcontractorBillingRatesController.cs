using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using Northwind.Web.Helpers;
using Northwind.Core.Interfaces.ContractRefactor;
using Northwind.Web.Models.ViewModels;
using Northwind.Web.Infrastructure.Helpers;
using NLog;
using Northwind.Web.Infrastructure.AuditLog;
using Northwind.Web.Infrastructure.Authorization;
using static Northwind.Core.Entities.EnumGlobal;
using Northwind.Web.Infrastructure.Models;

namespace Northwind.Web.Controllers
{
    public class SubcontractorBillingRatesController : Controller
    {
        private readonly IFileService _fileService;
        private readonly IConfiguration _configuration;
        private readonly ISubcontractorBillingRatesService _subcontractorBillingRatesService;
        private readonly IContractsService _contractRefactorService;
        private readonly IMapper _mapper;
        private readonly Logger _logger;
        private readonly IUserService _userService;
        private readonly IContractResourceFileService _contractResourceFileService;
        string documentRoot;
        string filePath;
        public SubcontractorBillingRatesController(
            IFileService fileService,
            IConfiguration configuration,
            ISubcontractorBillingRatesService subcontractorBillingRatesService,
            IContractsService contractRefactorService,
            IContractResourceFileService contractResourceFileService,
            IUserService userService,
            IMapper mapper
            )
        {
            _fileService = fileService;
            _configuration = configuration;
            _contractRefactorService = contractRefactorService;
            _subcontractorBillingRatesService = subcontractorBillingRatesService;
            _mapper = mapper;
            _userService = userService;
            _contractResourceFileService = contractResourceFileService;
            documentRoot = configuration.GetSection("Document").GetValue<string>("DocumentRoot");
            _logger = LogManager.GetCurrentClassLogger();
        }

        [HttpGet]
        [Secure(ResourceType.LaborCategoryRates, ResourceActionPermission.Add)]
        public ActionResult Add(Guid id)
        {
            try
            {
                var laborCategoryRates = new LaborCategoryRatesViewModel();
                Guid categoryRateGuid = Guid.NewGuid();
                var files = _contractRefactorService.GetFileByResourceGuid(id, Core.Entities.EnumGlobal.ResourceType.WorkBreakDownStructure.ToString());
                if (files != null)
                {
                    if (files.IsFile)
                        categoryRateGuid = files.ContentResourceGuid;
                }
                laborCategoryRates.CategoryRateGuid = categoryRateGuid;
                laborCategoryRates.ContractGuid = id;

                return PartialView(laborCategoryRates);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(ex.ToString(), ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPost]
        [Secure(ResourceType.LaborCategoryRates, ResourceActionPermission.Add)]
        public IActionResult Add([FromForm]LaborCategoryRatesViewModel laborCategoryRates)
        {
            try
            {
                if (laborCategoryRates.FileToUpload == null)
                {
                    ModelState.AddModelError("", "Please insert the file.");
                    return BadRequest(ModelState);
                }
                var isfileValid = _fileService.UploadFileTypeCheck(laborCategoryRates.FileToUpload);
                laborCategoryRates.CreatedOn = CurrentDateTimeHelper.GetCurrentDateTime();
                laborCategoryRates.UpdatedOn = CurrentDateTimeHelper.GetCurrentDateTime();
                laborCategoryRates.CreatedBy = UserHelper.CurrentUserGuid(HttpContext);
                laborCategoryRates.UpdatedBy = UserHelper.CurrentUserGuid(HttpContext);
                laborCategoryRates.IsActive = true;
                laborCategoryRates.IsDeleted = false;
                var contractNumber = _contractRefactorService.GetContractNumberById(laborCategoryRates.ContractGuid);
                var contractResourceFile = _contractResourceFileService.GetFilePathByResourceIdAndKeys(Core.Entities.EnumGlobal.ResourceType.LaborCategoryRates.ToString(), laborCategoryRates.ContractGuid);
                if (contractResourceFile != null && (laborCategoryRates.FileToUpload != null || laborCategoryRates.FileToUpload.Length != 0))
                {
                    var filename = "";
                    if (!isfileValid)
                    {
                        laborCategoryRates.IsCsv = false;
                        filename = _fileService.FilePost($@"{documentRoot}/{contractResourceFile.FilePath}/", laborCategoryRates.FileToUpload);
                        filePath = $"{contractResourceFile.FilePath}/{filename}";
                    }
                    else
                    {
                        var file = _contractRefactorService.GetFileByResourceGuid(laborCategoryRates.ContractGuid, Core.Entities.EnumGlobal.ResourceType.LaborCategoryRates.ToString());
                        var previousFile = string.Empty;
                        var relativePath = $@"{documentRoot}/{contractResourceFile.FilePath}/";
                        if (file != null)
                        {
                            previousFile = file.UploadFileName;
                        }
                        filename = _fileService.MoveFile(relativePath, previousFile, laborCategoryRates.FileToUpload);
                        var fullPath = $@"{relativePath}/{filename}";
                        Helpers.CsvValidationHelper.ChecksValidHeaderAndReadTheFile(fullPath, relativePath, previousFile, (Models.ViewModels.EnumGlobal.UploadMethodName)UploadMethodName.SubcontractorLaborBillingRates);
                        filePath = $"{contractResourceFile.FilePath}/{filename}";
                        laborCategoryRates.IsCsv = true;

                    }
                    laborCategoryRates.FilePath = filePath;
                    laborCategoryRates.UploadFileName = filename;

                    var contractFile = ContractsMapper.MapSubcontractorBillingRatesViewModelToContractFiles(laborCategoryRates);
                    contractFile.ResourceType = Core.Entities.EnumGlobal.ResourceType.Contract.ToString();
                    if (contractResourceFile != null)
                        contractFile.ParentId = contractResourceFile.ContractResourceFileGuid;
                    _contractRefactorService.CheckAndInsertContractFile(contractFile);

                    //audit log..
                    var contractEntity = _contractRefactorService.GetContractEntityByContractId(contractFile.ResourceGuid);
                    var additionalInformation = string.Format("{0} {1} the {2}", User.FindFirst("fullName").Value, CrudTypeForAdditionalLogMessage.Uploaded.ToString(), "Labor Category Rates File");
                    string additionalInformationURl = string.Empty;
                    string resource = string.Empty;

                    if (contractEntity.ParentContractGuid == Guid.Empty || contractEntity.ParentContractGuid == null)
                    {
                        additionalInformationURl = _configuration.GetSection("SiteUrl").Value + ("/Contract/Details/" + contractEntity.ContractGuid);
                        resource = string.Format("{0} </br> Contract No:{1} </br> Project No:{2}  </br> File Name:{3}", "Labor Category Rates", contractEntity.ContractNumber, contractEntity.ProjectNumber, laborCategoryRates.UploadFileName);
                    }
                    else
                    {
                        additionalInformationURl = _configuration.GetSection("SiteUrl").Value + ("/Project/Details/" + contractEntity.ContractGuid);
                        resource = string.Format("{0} </br> TaskOrder No:{1} </br> Project No:{2} </br> File Name:{3}", "Labor Category Rates", contractEntity.ContractNumber, contractEntity.ProjectNumber, laborCategoryRates.UploadFileName);
                    }

                    AuditLogHandler.InfoLog(_logger, User.FindFirst("fullName").Value, UserHelper.CurrentUserGuid(HttpContext), contractFile, resource, contractFile.ContractResourceFileGuid, UserHelper.GetHostedIp(HttpContext), "File Uploaded", Guid.Empty, "Successful", "", additionalInformation, additionalInformationURl);
                    //end of audit log.

                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                return Ok(new { status = ResponseStatus.success.ToString(), message = "Successfully Added !!" });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(ex.ToString(), ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpGet]
        [Secure(ResourceType.LaborCategoryRates, ResourceActionPermission.Details)]
        public ActionResult Detail(Guid id)
        {
            try
            {
                var file = _contractRefactorService.GetFileByResourceGuid(id, Core.Entities.EnumGlobal.ResourceType.LaborCategoryRates.ToString());
                var model = _mapper.Map<LaborCategoryRatesViewModel>(file);
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

        [HttpGet]
        [Secure(ResourceType.LaborCategoryRates, ResourceActionPermission.Details)]
        public IActionResult Get(Guid id)
        {
            try
            {

                var file = _contractRefactorService.GetFileByResourceGuid(id, Core.Entities.EnumGlobal.ResourceType.LaborCategoryRates.ToString());
                var model = _mapper.Map<LaborCategoryRatesViewModel>(file);

                if (model != null)
                {
                    var filePath = $"{documentRoot}{model.FilePath}";
                    var data = CsvValidationHelper.ReadFile(filePath, (Models.ViewModels.EnumGlobal.UploadMethodName)UploadMethodName.SubcontractorLaborBillingRates);
                    var listdata = data.Select(x => new
                    {
                        ContractGuid = id,
                        SubContractor = x.SubContractor,
                        LaborCode = x.LaborCode,
                        EmployeeName = x.EmployeeName,
                        Rate = x.Rate,
                        StartDate = x.StartDate,
                        EndDate = x.EndDate
                    }).ToList();
                    return Json(new { data = listdata });
                }
                IEnumerable<dynamic> nullList = new List<dynamic>();
                return Json(new { data = nullList });

            }
            catch (Exception ex)
            {
                ModelState.AddModelError(ex.ToString(), ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPost]
        [Secure(ResourceType.LaborCategoryRates, ResourceActionPermission.Details)]
        public IActionResult Get([FromBody] string searchText)
        {
            try
            {
                var list = JsonConvert.DeserializeObject<List<GridHeaderModel>>(searchText);
                if (list.Count == 0)
                    return BadRequest(new { status = ResponseStatus.error.ToString(), path = " : No Any Data In The Row" });
                var contractGuid = list[0].ContractGuid;
                var model = _contractRefactorService.GetFileByResourceGuid(contractGuid, Core.Entities.EnumGlobal.ResourceType.LaborCategoryRates.ToString());
                var contractResourceFile = _contractResourceFileService.GetFilePathByResourceIdAndKeys(Core.Entities.EnumGlobal.ResourceType.WorkBreakDownStructure.ToString(), contractGuid);
                string fileName = string.Format(model.UploadFileName, System.AppContext.BaseDirectory);
                var listdata = list.Select(x => new
                {
                    SubContractor = x.SubContractor,
                    LaborCode = x.LaborCode,
                    EmployeeName = x.EmployeeName,
                    Rate = x.Rate,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate
                }).ToList();
                var dynamicList = listdata.Cast<dynamic>().ToList();

                // saves the files to the folder specified 
                var relativePath = $@"{documentRoot}/{contractResourceFile.FilePath}/";
                var fullPath = $@"{relativePath}/{fileName}";
                CsvValidationHelper.MoveFile(relativePath, fileName);
                Helpers.CsvValidationHelper.SaveTheUpdatedCsv(dynamicList, fullPath);
                var previousFile = string.Empty;
                if (model != null)
                {
                    previousFile = model.UploadFileName;
                }
                // after file save the saved file is valid if not valid the file is deleted 
                Helpers.CsvValidationHelper.ChecksValidHeaderAndReadTheFile(fullPath, relativePath, previousFile, (Models.ViewModels.EnumGlobal.UploadMethodName)UploadMethodName.SubcontractorLaborBillingRates);

                //updates the new filename/path to the database
                model.FilePath = contractResourceFile.FilePath + "/" + fileName; ;
                model.UploadFileName = fileName;
                model.IsCsv = true;
                model.IsFile = true;
                _contractRefactorService.CheckAndInsertContractFile(model);


                return Ok(new { status = ResponseStatus.success.ToString(), id = model.ContractResourceFileGuid });
            }
            catch (Exception ex)
            {
                return BadRequestFormatter.BadRequest(this, ex);
            }
        }

        [HttpGet]
        [Secure(ResourceType.LaborCategoryRates, ResourceActionPermission.Edit)]
        public ActionResult Edit(Guid id)
        {
            try
            {
                var files = _contractRefactorService.GetFileByResourceGuid(id, Core.Entities.EnumGlobal.ResourceType.LaborCategoryRates.ToString());
                var model = _mapper.Map<LaborCategoryRatesViewModel>(files);
                var user = _userService.GetUserByUserGuid(files.UpdatedBy);
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
        [Secure(ResourceType.LaborCategoryRates, ResourceActionPermission.Edit)]
        public IActionResult Edit([FromForm]LaborCategoryRatesViewModel laborCategoryRates)
        {
            try
            {
                laborCategoryRates.UpdatedOn = CurrentDateTimeHelper.GetCurrentDateTime();
                laborCategoryRates.UpdatedBy = UserHelper.CurrentUserGuid(HttpContext);
                laborCategoryRates.IsActive = true;
                laborCategoryRates.IsDeleted = false;
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var contractFile = ContractsMapper.MapSubcontractorBillingRatesViewModelToContractFiles(laborCategoryRates);
                _contractRefactorService.UpdateContractFile(contractFile);

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