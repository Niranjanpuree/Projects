using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
using Northwind.Web.Models.ViewModels.Contract;
using static Northwind.Core.Entities.EnumGlobal;

namespace Northwind.Web.Controllers
{
    public class WorkBreakDownStructureController : Controller
    {
        private readonly IFileService _fileService;
        private readonly IConfiguration _configuration;
        private readonly IContractWBSService _contractWBSService;
        private readonly IMapper _mapper;
        private readonly IContractsService _contractRefactorService;
        private readonly IContractResourceFileService _contractResourceFileService;
        private readonly IUserService _userService;
        private readonly Logger _logger;
        string documentRoot;
        public WorkBreakDownStructureController(
            IFileService fileService,
            IConfiguration configuration,
            IContractWBSService contractWBSService,
            IContractsService contractRefactorService,
            IUserService userService,
            IContractResourceFileService contractResourceFileService,
            IMapper mapper
            )
        {
            _fileService = fileService;
            _configuration = configuration;
            _contractRefactorService = contractRefactorService;
            _contractWBSService = contractWBSService;
            _mapper = mapper;
            _contractResourceFileService = contractResourceFileService;
            documentRoot = configuration.GetSection("Document").GetValue<string>("DocumentRoot");
            _userService = userService;
            _logger = LogManager.GetCurrentClassLogger();
        }

        [HttpGet]
        [Secure(ResourceType.WorkBreakDownStructure, ResourceActionPermission.Add)]
        public ActionResult Add(Guid id)
        {
            try
            {
                var contractWBS = new ContractWBSViewModel();
                Guid ContractWBSGuid = Guid.NewGuid();
                var files = _contractRefactorService.GetFileByResourceGuid(id, Core.Entities.EnumGlobal.ResourceType.WorkBreakDownStructure.ToString());
                if(files!=null)
                {
                    if(files.IsFile)
                    ContractWBSGuid = files.ContentResourceGuid;
                }

                contractWBS.ContractWBSGuid = ContractWBSGuid;
                contractWBS.ContractGuid = id;

                return PartialView(contractWBS);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(ex.ToString(), ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpGet]
        [Secure(ResourceType.WorkBreakDownStructure, ResourceActionPermission.Details)]
        public ActionResult Detail(Guid id)
        {
            try
            {
                //add file to contract file
                var file = _contractRefactorService.GetFileByResourceGuid(id, Core.Entities.EnumGlobal.ResourceType.WorkBreakDownStructure.ToString());
                var model = _mapper.Map<ContractWBSViewModel>(file);
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
        [Secure(ResourceType.WorkBreakDownStructure, ResourceActionPermission.Edit)]
        public ActionResult Edit(Guid id)
        {
            try
            {
                //edited
                var files = _contractRefactorService.GetFileByResourceGuid(id, Core.Entities.EnumGlobal.ResourceType.WorkBreakDownStructure.ToString());
                var model = _mapper.Map<ContractWBSViewModel>(files);
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

        /// <summary>
        /// Get the list of Csv For the Grid list
        /// </summary>
        [HttpGet]
        [Secure(ResourceType.WorkBreakDownStructure, ResourceActionPermission.Details)]
        public IActionResult Get(Guid id)
        {
            try
            {
                var file = _contractRefactorService.GetFileByResourceGuid(id, Core.Entities.EnumGlobal.ResourceType.WorkBreakDownStructure.ToString());
                if (file != null)
                {
                    var model = _mapper.Map<ContractWBSViewModel>(file);
                    var filePath = $"{documentRoot}/{model.FilePath}";
                    var data = CsvValidationHelper.ReadFile(filePath, (Models.ViewModels.EnumGlobal.UploadMethodName)UploadMethodName.WorkBreakDownStructure);
                    var listdata = data.Select(x => new
                    {
                        ContractGuid = id,
                        WBSCode = x.WBSCode,
                        Description = x.Description,
                        Value = x.Value,
                        ContractType = x.ContractType,
                        InvoiceAtThisLevel = x.InvoiceAtThisLevel
                    }).ToList();
                    return Json(new { data = listdata });
                }
                return Json(new { data = new List<dynamic>() });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(ex.ToString(), ex.Message);
                return BadRequest(ModelState);
            }
        }

        [Authorize]
        public PartialViewResult GetWbsView(Guid id)
        {
            var file = _contractRefactorService.GetFileByResourceGuid(id, Core.Entities.EnumGlobal.ResourceType.WorkBreakDownStructure.ToString());
            var model = _mapper.Map<ContractWBSViewModel>(file);
            return PartialView("Detail", model);
        }

        [HttpPost]
        [Secure(ResourceType.WorkBreakDownStructure, ResourceActionPermission.Add)]
        public IActionResult Add([FromForm]ContractWBSViewModel contractWBS)
        {
            try
            {
                if(contractWBS.FileToUpload == null)
                {
                    ModelState.AddModelError("", "Please insert the file.");
                    return BadRequest(ModelState);
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                contractWBS.CreatedOn = CurrentDateTimeHelper.GetCurrentDateTime();
                contractWBS.UpdatedOn = CurrentDateTimeHelper.GetCurrentDateTime();
                contractWBS.CreatedBy = UserHelper.CurrentUserGuid(HttpContext);
                contractWBS.UpdatedBy = UserHelper.CurrentUserGuid(HttpContext);
                contractWBS.IsActive = true;
                contractWBS.IsDeleted = false;
                contractWBS.IsCsv = true;
                string filePath = string.Empty;
                // gets the contractnumber to save the file in the folder.
                var contractNumber = _contractRefactorService.GetContractNumberById(contractWBS.ContractGuid);
                var contractResourceFile = _contractResourceFileService.GetFilePathByResourceIdAndKeys(EnumGlobal.ResourceType.WorkBreakDownStructure.ToString(), contractWBS.ContractGuid);
                if (contractWBS.FileToUpload != null || contractWBS.FileToUpload.Length != 0)
                {
                    var filename = "";
                    //checks whether the file extension is the correct one and the validates the fields if the file is Csv.
                    var isfileValid = _fileService.UploadFileTypeCheck(contractWBS.FileToUpload);
                    if (!isfileValid)
                    {
                        filename = _fileService.FilePost($@"{documentRoot}/{contractResourceFile.FilePath}/", contractWBS.FileToUpload);
                        contractWBS.IsCsv = false;
                        filePath = $"{contractResourceFile.FilePath}/{filename}";
                    }
                    else
                    {
                        var files = _contractRefactorService.GetFileByResourceGuid(contractWBS.ContractGuid, Core.Entities.EnumGlobal.ResourceType.WorkBreakDownStructure.ToString());
                        var previousFile = string.Empty;
                        var relativePath = $@"{documentRoot}/{contractResourceFile.FilePath}/";
                        if (files != null)
                        {
                            previousFile = files.UploadFileName;
                        }
                        filename = _fileService.MoveFile(relativePath, previousFile, contractWBS.FileToUpload);
                        var fullPath = $@"{relativePath}/{filename}";
                        CsvValidationHelper.ChecksValidHeaderAndReadTheFile(fullPath, relativePath, previousFile, (Models.ViewModels.EnumGlobal.UploadMethodName)UploadMethodName.WorkBreakDownStructure);
                        filePath = $"{contractResourceFile.FilePath}/{filename}";
                        contractWBS.IsCsv = true;
                    }
                    contractWBS.FilePath = filePath;
                    contractWBS.UploadFileName = filename;

                    var contractFile = ContractsMapper.MapContractWBSViewModelToContractResourceFile(contractWBS);
                    contractFile.ResourceType = EnumGlobal.ResourceType.Contract.ToString();
                    if (contractResourceFile != null)
                        contractFile.ParentId = contractResourceFile.ContractResourceFileGuid;
                    _contractRefactorService.CheckAndInsertContractFile(contractFile);

                    //audit log..
                    var contractEntity = _contractRefactorService.GetContractEntityByContractId(contractFile.ResourceGuid);
                    var additionalInformation = string.Format("{0} {1} the {2}", User.FindFirst("fullName").Value, CrudTypeForAdditionalLogMessage.Uploaded.ToString(), "Work BreakDown Structure File");
                    string additionalInformationURl = string.Empty;
                    string resource = string.Empty;

                    if (contractEntity.ParentContractGuid == Guid.Empty || contractEntity.ParentContractGuid == null)
                    {
                        additionalInformationURl = _configuration.GetSection("SiteUrl").Value + ("/Contract/Details/" + contractEntity.ContractGuid);
                        resource = string.Format("{0} </br>  Contract No:{1} </br> Project No:{2}  </br> File Name:{3}", "Work BreakDown Structure", contractEntity.ContractNumber, contractEntity.ProjectNumber, contractWBS.UploadFileName);
                    }
                    else
                    {
                        additionalInformationURl = _configuration.GetSection("SiteUrl").Value + ("/Project/Details/" + contractEntity.ContractGuid);
                        resource = string.Format("{0} </br> TaskOrder No:{1} </br> Project No:{2}  </br> File Name:{3}", "Work BreakDown Structure", contractEntity.ContractNumber, contractEntity.ProjectNumber, contractWBS.UploadFileName);
                    }

                    AuditLogHandler.InfoLog(_logger, User.FindFirst("fullName").Value, UserHelper.CurrentUserGuid(HttpContext), contractFile, resource, contractFile.ContractResourceFileGuid, UserHelper.GetHostedIp(HttpContext), "File Uploaded", Guid.Empty, "Successful", "", additionalInformationURl, additionalInformationURl);
                    //end of audit log.
                }
                //add file to contract file

                return Ok(new { status = ResponseStatus.success.ToString(), message = "Successfully Added !!" });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(ex.ToString(), ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPost]
        [Secure(ResourceType.WorkBreakDownStructure, ResourceActionPermission.Edit)]
        public IActionResult Edit([FromForm]ContractWBSViewModel contractWBS)
        {
            try
            {
                contractWBS.UpdatedOn = CurrentDateTimeHelper.GetCurrentDateTime();
                contractWBS.UpdatedBy = UserHelper.CurrentUserGuid(HttpContext);
                contractWBS.IsActive = true;
                contractWBS.IsDeleted = false;
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                //var wbsEntity = _mapper.Map<ContractWBS>(contractWBS);
                //_contractWBSService.UpdateContractWBS(wbsEntity);

                //add file to contract file
                var contractFile = ContractsMapper.MapContractWBSViewModelToContractResourceFile(contractWBS);
                _contractRefactorService.UpdateContractFile(contractFile);
                //end of contract file

                return Ok(new { status = ResponseStatus.success.ToString(), message = "Successfully Edited !!" });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(ex.ToString(), ex.Message);
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Checks the Inline Edited Grid Data and converts to CSV and saves in the folder. 
        /// </summary>
        [HttpPost]
        [Secure(ResourceType.WorkBreakDownStructure, ResourceActionPermission.Details)]
        public IActionResult Get([FromBody] string searchText)
        {
            try
            {
                var list = JsonConvert.DeserializeObject<List<GridHeaderModel>>(searchText);
                if (list.Count == 0)
                    return BadRequest(new { status = ResponseStatus.error.ToString(), path = " : No Any Data In The Row" });
                string filePath = string.Empty;
                var contractGuid = list[0].ContractGuid;
                var model = _contractRefactorService.GetFileByResourceGuid(contractGuid, Core.Entities.EnumGlobal.ResourceType.WorkBreakDownStructure.ToString());
                var contractResourceFile = _contractResourceFileService.GetFilePathByResourceIdAndKeys(Core.Entities.EnumGlobal.ResourceType.WorkBreakDownStructure.ToString(), contractGuid);
                string fileName = string.Format(model.UploadFileName, System.AppContext.BaseDirectory);
                var listdata = list.Select(x => new
                {
                    WBSCode = x.WBSCode,
                    Description = x.Description,
                    Value = x.Value,
                    ContractType = x.ContractType,
                    InvoiceAtThisLevel = x.InvoiceAtThisLevel
                }).ToList();
                var dynamicList = listdata.Cast<dynamic>().ToList();

                // saves the files to the folder specified 
                var relativePath = $@"{documentRoot}/{contractResourceFile.FilePath}/";
                var fullPath = $@"{relativePath }/{fileName}";
                CsvValidationHelper.MoveFile(relativePath, fileName);
                CsvValidationHelper.SaveTheUpdatedCsv(dynamicList, fullPath);
                var previousFile = string.Empty;
                if (model != null)
                {
                    previousFile = model.UploadFileName;
                }
                // after file save the saved file is valid if not valid the file is deleted 
                CsvValidationHelper.ChecksValidHeaderAndReadTheFile(fullPath, relativePath, previousFile, (Models.ViewModels.EnumGlobal.UploadMethodName)UploadMethodName.WorkBreakDownStructure);

                //updates the new filename/path to the database
                model.FilePath = contractResourceFile.FilePath + "/" + fileName;
                model.UploadFileName = fileName;
                model.IsCsv = true;
                model.IsFile = true;
                _contractRefactorService.CheckAndInsertContractFile(model);

                return Ok(new { status = ResponseStatus.success.ToString(), id = model.ContractResourceFileGuid });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}