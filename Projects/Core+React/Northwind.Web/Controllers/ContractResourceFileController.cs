using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NLog;
using Northwind.Core.Entities;
using Northwind.Core.Entities.ContractRefactor;
using Northwind.Core.Entities.DocumentMgmt;
using Northwind.Core.Interfaces;
using Northwind.Core.Interfaces.ContractRefactor;
using Northwind.Core.Interfaces.DocumentMgmt;
using Northwind.Web.Helpers;
using Northwind.Web.Infrastructure.AuditLog;
using Northwind.Web.Infrastructure.Helpers;
using Northwind.Web.Infrastructure.Models;
using Northwind.Web.Models.ViewModels.Contract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using static Northwind.Core.Entities.EnumGlobal;
using Microsoft.AspNetCore.Authorization;

namespace Northwind.Web.Controllers
{
    public class ContractResourceFileController : Controller
    {
        private readonly IContractsService _contractsService;
        private readonly IDocumentManagementService _documentManagementService;
        private readonly IFolderStructureMasterService _folderStructureMasterService;
        private readonly IFolderStructureFolderService _folderStructureFolderService;
        private readonly IContractModificationService _contractModificationService;
        private readonly IFileService _fileService;
        private readonly IConfiguration _configuration;
        private readonly IContractsService _contractRefactorService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        string documentRoot;
        private readonly Logger _logger;
        private readonly IContractResourceFileService _contractResourceFileService;

        public ContractResourceFileController(
            IContractsService contractsService,
            IDocumentManagementService documentManagementService,
            IFolderStructureMasterService folderStructureMasterService,
            IFolderStructureFolderService folderStructureFolderService,
            IContractModificationService contractModificationService,
            IFileService fileService,
            IConfiguration configuration,
            IContractsService contractRefactorService,
            IUserService userService,
            IContractResourceFileService contractResourceFileService,
            IMapper mapper)
        {
            _contractsService = contractsService;
            _documentManagementService = documentManagementService;
            _folderStructureMasterService = folderStructureMasterService;
            _folderStructureFolderService = folderStructureFolderService;
            _contractModificationService = contractModificationService;
            _fileService = fileService;
            _configuration = configuration;
            _contractRefactorService = contractRefactorService;
            _mapper = mapper;
            _userService = userService;
            documentRoot = configuration.GetSection("Document").GetValue<string>("DocumentRoot");
            _logger = LogManager.GetCurrentClassLogger();
            _contractResourceFileService = contractResourceFileService;
        }

        private IDocumentEntity GetTemplateFolderTree(Guid masterFolderGuid, string resourceType, Guid resourceId, string pathPrefixName)
        {
            var structureApplied = _documentManagementService.HasDefaultStructure(resourceType, resourceId);
            var contract = _contractsService.GetContractEntityByContractId(resourceId);

            if (!structureApplied && contract != null)
            {
                var masterData = _folderStructureMasterService.GetActive("ESSWeb", resourceType);
                if (masterData.Count() > 0)
                {
                    var templateFolders = _folderStructureFolderService.GetFolderTree(masterData.SingleOrDefault().FolderStructureMasterGuid);
                    _documentManagementService.ManageDefaultStructure(resourceId.ToString(), contract.ContractNumber, templateFolders, resourceType, resourceId, UserHelper.CurrentUserGuid(HttpContext));
                }
            }
            var folders = _documentManagementService.GetFolderByMasterFolderGuid(masterFolderGuid, resourceType, resourceId);
            return folders;
        }

        [HttpPost]
        [DisableRequestSizeLimit]
        [Authorize]
        public IActionResult UploadSaveFileInFolderTreeNode(Guid resourceId, string resourceKey, string pathPrefixName, bool isTreeTemplate, string[] fileInfo)
        {
            try
            {
                //Avoiding Kendo Uploader's default call
                if (resourceId == Guid.Empty || fileInfo.Length == 0)
                {
                    return Ok(true);
                }

                var fileInfoObj1 = fileInfo[0].Split(',');
                IDocumentEntity documentEntity = new DocumentEntity();
                if (isTreeTemplate)
                    documentEntity = GetTemplateFolderTree(new Guid(fileInfoObj1[5]), "Contract", resourceId, pathPrefixName);
                else
                    documentEntity = _documentManagementService.GetFileOrFolder(new Guid(fileInfoObj1[5]));


                //If new file/s have been uploaded
                if (Request.Form.Files.Count > 0)
                {
                    var files = Request.Form.Files;
                    //foreach (var file in files)
                    for (int i = 0; i < files.Count; i++)
                    {
                        var fileInfoObj = fileInfo[i].Split(',');

                        //save file in folder..

                        var fullPath = files[i].FileName.Split("\\");
                        var originalFileName = fullPath[fullPath.Length - 1];

                        //var fullPhysicalFilePath = Path.Combine(documentRoot, newTreeStructure.FilePath);
                        var fullPhysicalFilePath = string.Concat(documentRoot, documentEntity.FilePath);

                        var uniqueFileName =
                            _fileService.SaveFile(fullPhysicalFilePath, files[i]);

                        var relativeFilePath = Path.Combine(documentEntity.FilePath, uniqueFileName);

                        //save file info in database..
                        ContractFileViewModel contractFileViewModel = new ContractFileViewModel();

                        contractFileViewModel.ContractResourceFileGuid = new Guid(fileInfoObj[0]);
                        contractFileViewModel.CreatedOn = CurrentDateTimeHelper.GetCurrentDateTime();
                        contractFileViewModel.UpdatedOn = CurrentDateTimeHelper.GetCurrentDateTime();
                        contractFileViewModel.CreatedBy = UserHelper.CurrentUserGuid(HttpContext);
                        contractFileViewModel.UpdatedBy = UserHelper.CurrentUserGuid(HttpContext);
                        contractFileViewModel.Description = WebUtility.UrlDecode(fileInfoObj[1]);
                        contractFileViewModel.MimeType = files[i].ContentType;
                        contractFileViewModel.ResourceGuid = resourceId;
                        contractFileViewModel.keys = resourceKey;
                        contractFileViewModel.UploadFileName = uniqueFileName;
                        contractFileViewModel.UploadUniqueFileName = uniqueFileName;
                        contractFileViewModel.IsActive = true;
                        contractFileViewModel.IsDeleted = false;
                        contractFileViewModel.FilePath = relativeFilePath;
                        contractFileViewModel.FileSize = fileInfoObj[4];
                        contractFileViewModel.ParentId = documentEntity.ContractResourceFileGuid;
                        contractFileViewModel.Isfile = true;

                        var fileEntity = _mapper.Map<ContractResourceFile>(contractFileViewModel);

                        _contractRefactorService.InsertContractFile(fileEntity);

                        //auditlog..
                        AuditLogForUploadDownload(fileEntity, CrudTypeForAdditionalLogMessage.Uploaded.ToString());
                    }
                }

                //If existing file/s description have been changed
                if (fileInfo.Length > Request.Form.Files.Count)
                {
                    for (int i = Request.Form.Files.Count; i <= fileInfo.Length - 1; i++)
                    {
                        var fileInfoObj = fileInfo[i].Split(',');

                        var fileById = _contractRefactorService.GetFilesByContractFileGuid(new Guid(fileInfoObj[0]));
                        if (fileById != null)
                        {
                            fileById.Description = WebUtility.UrlDecode(fileInfoObj[1]);
                            _contractRefactorService.UpdateFile(fileById);
                        }
                    }
                }

                return Ok(true);
            }
            catch (Exception ex)
            {
                return BadRequestFormatter.BadRequest(this, ex);
            }
        }

        [HttpPost]
        [DisableRequestSizeLimit]
        [Authorize]
        public IActionResult UploadSaveFile(Guid resourceId, string resourceKey, string uploadPath, string[] fileInfo, Guid parentid, string contractResourceFileKey, Guid ContentResourceGuid)
        {
            try
            {

                //Avoiding Kendo Uploader's default call
                if (resourceId == Guid.Empty || string.IsNullOrEmpty(uploadPath))
                {
                    return Ok(true);
                }

                //If new file/s have been uploaded
                if (Request.Form.Files.Count > 0)
                {
                    var files = Request.Form.Files;
                    //foreach (var file in files)
                    for (int i = 0; i < files.Count; i++)
                    {
                        var fileInfoObj = fileInfo[i].Split(',');

                        //save file in folder..

                        var fullPath = files[i].FileName.Split("\\");
                        var originalFileName = fullPath[fullPath.Length - 1];

                        //var fullPhysicalFilePath = Path.Combine(documentRoot, uploadPath);
                        var fullPhysicalFilePath = string.Concat(documentRoot, uploadPath);

                        var uniqueFileName =
                            _fileService.SaveFile(fullPhysicalFilePath, files[i]);

                        var relativeFilePath = Path.Combine(uploadPath, uniqueFileName);

                        //save file info in database..
                        ContractFileViewModel contractFileViewModel = new ContractFileViewModel();

                        contractFileViewModel.ContractResourceFileGuid = new Guid(fileInfoObj[0]);
                        contractFileViewModel.CreatedOn = CurrentDateTimeHelper.GetCurrentDateTime();
                        contractFileViewModel.UpdatedOn = CurrentDateTimeHelper.GetCurrentDateTime();
                        contractFileViewModel.CreatedBy = UserHelper.CurrentUserGuid(HttpContext);
                        contractFileViewModel.UpdatedBy = UserHelper.CurrentUserGuid(HttpContext);
                        contractFileViewModel.Description = WebUtility.UrlDecode(fileInfoObj[1]);
                        contractFileViewModel.MimeType = files[i].ContentType;
                        contractFileViewModel.ResourceGuid = resourceId;
                        //contractFileViewModel.keys = resourceKey;
                        contractFileViewModel.ParentId = parentid;
                        contractFileViewModel.UploadFileName = uniqueFileName;
                        contractFileViewModel.UploadUniqueFileName = uniqueFileName;
                        contractFileViewModel.IsActive = true;
                        contractFileViewModel.IsDeleted = false;
                        contractFileViewModel.FilePath = relativeFilePath;
                        contractFileViewModel.FileSize = fileInfoObj[4];
                        contractFileViewModel.Isfile = true;
                        contractFileViewModel.keys = contractResourceFileKey;
                        contractFileViewModel.ResourceType = resourceKey;
                        contractFileViewModel.ContentResourceGuid = ContentResourceGuid;

                        if (resourceKey.ToUpper() == EnumGlobal.ResourceType.ContractModification.ToString().ToUpper() || resourceKey.ToUpper() == EnumGlobal.ResourceType.TaskOrderModification.ToString().ToUpper())
                        {
                            var contractGuid = _contractModificationService.GetDetailById(ContentResourceGuid).ContractGuid;
                            var contractResourceFile = _contractResourceFileService.GetFilePathByResourceIdAndKeys(ContractResourceFileKey.ContractModification.ToString(), contractGuid);
                            contractFileViewModel.ParentId = contractResourceFile.ContractResourceFileGuid;
                            contractFileViewModel.keys = ContractResourceFileKey.ContractModification.ToString();
                            contractFileViewModel.ResourceGuid = contractGuid;

                            //contractFileViewModel.ResourceType = resourceKey;
                            //for task order also resourcetype is Contract to show in the tree view from the database..
                            contractFileViewModel.ResourceType = EnumGlobal.ResourceType.Contract.ToString();
                        }
                        else if ((parentid == Guid.Empty || parentid != null) && ContentResourceGuid == Guid.Empty)
                        {
                            if (resourceKey.ToUpper() != EnumGlobal.ResourceType.ContractCloseOut.ToString().ToUpper())
                            {
                                var contractResourceFile = _contractResourceFileService.GetFilePathByResourceIdAndKeys(EnumGlobal.ResourceType.Base.ToString(), resourceId);
                                contractFileViewModel.ParentId = contractResourceFile.ContractResourceFileGuid;
                                contractFileViewModel.keys = EnumGlobal.ResourceType.Base.ToString();
                                //contractFileViewModel.ResourceType = resourceKey;
                                //for task order also resourcetype is Contract to show in the tree view from the database..
                                contractFileViewModel.ResourceType = EnumGlobal.ResourceType.Contract.ToString();
                            }
                        }

                        if (resourceKey.ToUpper() == EnumGlobal.ResourceType.ContractCloseOut.ToString().ToUpper())
                        {
                            contractFileViewModel.ResourceType = EnumGlobal.ResourceType.Contract.ToString();
                        }

                        var fileEntity = _mapper.Map<ContractResourceFile>(contractFileViewModel);

                        _contractRefactorService.InsertContractFile(fileEntity);

                        //auditlog..
                        AuditLogForUploadDownload(fileEntity, CrudTypeForAdditionalLogMessage.Uploaded.ToString());
                    }
                }

                //If existing file/s description have been changed
                if (fileInfo.Length > Request.Form.Files.Count)
                {
                    for (int i = Request.Form.Files.Count; i <= fileInfo.Length - 1; i++)
                    {
                        var fileInfoObj = fileInfo[i].Split(',');

                        var fileById = _contractRefactorService.GetFilesByContractFileGuid(new Guid(fileInfoObj[0]));
                        if (fileById != null)
                        {
                            fileById.Description = WebUtility.UrlDecode(fileInfoObj[1]);
                            _contractRefactorService.UpdateFile(fileById);
                        }
                    }
                }

                return Ok(true);
            }
            catch (Exception ex)
            {
                return BadRequestFormatter.BadRequest(this, ex);
            }
        }

        [HttpPost]
        public IActionResult UploadFile(Guid resourceId, string description)
        {
            var fileById = _contractRefactorService.GetFilesByContractFileGuid(resourceId);
            if (fileById != null)
            {
                fileById.Description = WebUtility.UrlDecode(description);
                _contractRefactorService.UpdateFile(fileById);
            }
            return Ok(true);
        }

        [HttpPost]
        [Authorize]
        public IActionResult DeleteUploadedFile(Guid ResourceId, string FilePath)
        {
            try
            {
                var fullPhysicalFilePath = Path.Combine(documentRoot, FilePath);

                _fileService.DeleteFileFromDirectory(fullPhysicalFilePath);
                _contractRefactorService.DeleteContractFileById(ResourceId);
                return Ok(true);
            }
            catch (Exception ex)
            {
                return BadRequestFormatter.BadRequest(this, ex);
            }
        }

        [HttpGet]
        [Authorize]
        public IActionResult DownloadDocument(Guid Id)
        {
            try
            {
                var file = _contractRefactorService.GetFilesByContractFileGuid(Id);
                var filePath = file.FilePath;
                var fileKey = file.Keys.ToLower();
                if (fileKey == "laborcategoryrates" || fileKey == "workbreakdownstructure" || fileKey == "employeebillingrates")
                {
                    filePath = $"{file.FilePath}/{file.UploadFileName}";
                }
                //var fullPhysicalFilePath = Path.Combine(documentRoot.ToString(), filePath);
                var fullPhysicalFilePath = string.Concat(documentRoot.ToString(), filePath);

                var fileExists = System.IO.File.Exists(fullPhysicalFilePath);

                if (fileExists)
                {
                    var downloadFile = _fileService.GetFile(file.UploadFileName, fullPhysicalFilePath);
                    var keys = file.Keys;

                    //auditlog..
                    AuditLogForUploadDownload(file, CrudTypeForAdditionalLogMessage.Downloaded.ToString());

                    return downloadFile;
                }
                else
                {
                    //return (IActionResult)NotFound();
                    return View(@"~/Views/Shared/FileNotFound.cshtml", file.UploadFileName);
                }
            }
            catch (Exception ex)
            {
                return BadRequestFormatter.BadRequest(this, ex);
            }
        }

        private void AuditLogForUploadDownload(ContractResourceFile file, string type)
        {
            var keys = file.Keys;
            string additionalInformation = string.Empty;
            string additionalInformationURl = string.Empty;
            string resource = string.Empty;

            if (keys.Equals(ResourceType.Contract.ToString()))
            {
                var contractEntity = _contractsService.GetContractEntityByContractId(file.ResourceGuid);
                additionalInformation = string.Format("{0} {1} the {2}", User.FindFirst("fullName").Value, type, ResourceType.Contract.ToString().ToLower() + " File");
                additionalInformationURl = _configuration.GetSection("SiteUrl").Value + ("/Contract/Details/" + contractEntity.ContractGuid);
                resource = string.Format("{0} </br>  Contract No:{1} </br> Project No:{2} </br> Contract Title:{3} </br> File Name:{4}", ResourceType.Contract.ToString(), contractEntity.ContractNumber, contractEntity.ProjectNumber, contractEntity.ContractTitle, file.UploadFileName);
            }
            else if (keys.Equals(ResourceType.TaskOrder.ToString()))
            {
                var contractEntity = _contractsService.GetContractEntityByContractId(file.ResourceGuid);
                additionalInformation = string.Format("{0} {1} the {2}", User.FindFirst("fullName").Value, type, ResourceType.TaskOrder.ToString().ToLower() + " File");
                additionalInformationURl = _configuration.GetSection("SiteUrl").Value + ("/Project/Details/" + contractEntity.ContractGuid);
                resource = string.Format("{0} </br>  TaskOrder No:{1} </br> Project No:{2} </br> TaskOrder Title:{3} </br> File Name:{4}", ResourceType.Contract.ToString(), contractEntity.ContractNumber, contractEntity.ProjectNumber, contractEntity.ContractTitle, file.UploadFileName);
            }
            else if (keys.Equals(ResourceType.ContractModification.ToString()))
            {
                var contractModificationEntity = _contractModificationService.GetDetailById(file.ContentResourceGuid);
                additionalInformation = string.Format("{0} {1} the {2}", User.FindFirst("fullName").Value, type, "Contract Mod File");
                additionalInformationURl = _configuration.GetSection("SiteUrl").Value + ("/Contract/Details/" + contractModificationEntity.ContractGuid);
                resource = string.Format("{0} </br> Mod No: {1} </br> Mod Title:{2}</br> File Name:{3}", "Contract Mod", contractModificationEntity.ModificationNumber, contractModificationEntity.ModificationTitle, file.UploadFileName);
            }
            else if (keys.Equals(ResourceType.TaskOrderModification.ToString()))
            {
                var contractModificationEntity = _contractModificationService.GetDetailById(file.ContentResourceGuid);
                additionalInformation = string.Format("{0} {1} the {2}", User.FindFirst("fullName").Value, type, "TaskOrder Mod File");
                additionalInformationURl = _configuration.GetSection("SiteUrl").Value + ("/Project/Details/" + contractModificationEntity.ContractGuid);
                resource = string.Format("{0} </br>  Mod No: {1} </br> Mod Title:{2}</br> File Name:{3}", "Contract Mod", contractModificationEntity.ModificationNumber, contractModificationEntity.ModificationTitle, file.UploadFileName);
            }
            else if (keys.Equals(ResourceType.EmployeeBillingRates.ToString()))
            {
                var contractEntity = _contractsService.GetContractEntityByContractId(file.ResourceGuid);
                additionalInformation = string.Format("{0} {1} the {2}", User.FindFirst("fullName").Value, type, "Employee Billing Rates File");

                if (contractEntity.ParentContractGuid == Guid.Empty || contractEntity.ParentContractGuid == null)
                {
                    additionalInformationURl = _configuration.GetSection("SiteUrl").Value + ("/Contract/Details/" + contractEntity.ContractGuid);
                    resource = string.Format("{0} </br>  Contract No:{1} </br> Project No:{2} </br> Contract Title:{3} </br> File Name:{4}", "Employee Billing Rates", contractEntity.ContractNumber, contractEntity.ProjectNumber, contractEntity.ContractTitle, file.UploadFileName);
                }
                else
                {
                    additionalInformationURl = _configuration.GetSection("SiteUrl").Value + ("/Project/Details/" + contractEntity.ContractGuid);
                    resource = string.Format("{0} </br>  TaskOrder No:{1} </br> Project No:{2} </br> TaskOrder Title:{3} </br> File Name:{4}", "Employee Billing Rates", contractEntity.ContractNumber, contractEntity.ProjectNumber, contractEntity.ContractTitle, file.UploadFileName);
                }
            }
            else if (keys.Equals(ResourceType.WorkBreakDownStructure.ToString()))
            {
                var contractEntity = _contractsService.GetContractEntityByContractId(file.ResourceGuid);
                additionalInformation = string.Format("{0} {1} the {2}", User.FindFirst("fullName").Value, type, "Work BreakDown Structure File");

                if (contractEntity.ParentContractGuid == Guid.Empty || contractEntity.ParentContractGuid == null)
                {
                    additionalInformationURl = _configuration.GetSection("SiteUrl").Value + ("/Contract/Details/" + contractEntity.ContractGuid);
                    resource = string.Format("{0} </br>  Contract No:{1} </br> Project No:{2} </br> Contract Title:{3} </br> File Name:{4}", "Work BreakDown Structure", contractEntity.ContractNumber, contractEntity.ProjectNumber, contractEntity.ContractTitle, file.UploadFileName);
                }
                else
                {
                    additionalInformationURl = _configuration.GetSection("SiteUrl").Value + ("/Project/Details/" + contractEntity.ContractGuid);
                    resource = string.Format("{0} </br>  TaskOrder No:{1} </br> Project No:{2} </br> TaskOrder Title:{3} </br> File Name:{4}", "Work BreakDown Structure", contractEntity.ContractNumber, contractEntity.ProjectNumber, contractEntity.ContractTitle, file.UploadFileName);
                }
            }
            else if (keys.Equals(ResourceType.LaborCategoryRates.ToString()))
            {
                var contractEntity = _contractsService.GetContractEntityByContractId(file.ResourceGuid);
                additionalInformation = string.Format("{0} {1} the {2}", User.FindFirst("fullName").Value, type, "Labor Category Rates File");

                if (contractEntity.ParentContractGuid == Guid.Empty || contractEntity.ParentContractGuid == null)
                {
                    additionalInformationURl = _configuration.GetSection("SiteUrl").Value + ("/Contract/Details/" + contractEntity.ContractGuid);
                    resource = string.Format("{0} </br>  Contract No:{1} </br> Project No:{2} </br> Contract Title:{3} </br> File Name:{4}", "Labor Category Rates", contractEntity.ContractNumber, contractEntity.ProjectNumber, contractEntity.ContractTitle, file.UploadFileName);
                }
                else
                {
                    additionalInformationURl = _configuration.GetSection("SiteUrl").Value + ("/Project/Details/" + contractEntity.ContractGuid);
                    resource = string.Format("{0} </br>  TaskOrder No:{1} </br> Project No:{2} </br> TaskOrder Title:{3} </br> File Name:{4}", "Labor Category Rates", contractEntity.ContractNumber, contractEntity.ProjectNumber, contractEntity.ContractTitle, file.UploadFileName);
                }
            }

            AuditLogHandler.InfoLog(_logger, User.FindFirst("fullName").Value, UserHelper.CurrentUserGuid(HttpContext), file, resource, file.ContractResourceFileGuid, UserHelper.GetHostedIp(HttpContext), "File " + type, Guid.Empty, "Successful", "", additionalInformation, additionalInformationURl);
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetFilesByResourceId(Guid resourceId)
        {
            try
            {
                var files = _contractRefactorService.GetFileListByContractGuid(resourceId);

                var listdata = files.Select(x => new
                {
                    contractResourceFileGuid = x.ContractResourceFileGuid,
                    ResourceGuid = x.ResourceGuid,
                    UploadFileName = x.UploadFileName,
                    Description = x.Description,
                    FilePath = x.FilePath,
                    FileSize = x.FileSize,
                    Keys = x.Keys,
                    UpdatedBy = GetUserFullName(x.UpdatedBy),
                    UpdatedOn = x.UpdatedOn.ToString("MM/dd/yyyy")
                }).ToList();
                return Json(listdata);

            }
            catch (Exception ex)
            {
                return BadRequestFormatter.BadRequest(this, ex);
            }
        }


        [HttpGet]
        public IActionResult GetFilesByContentResourceId(Guid resourceId)
        {
            try
            {
                var files = new System.Collections.Generic.List<ContractResourceFile>();
                if (resourceId != Guid.Empty)
                {
                    files = _contractRefactorService.GetFileListByContentResourceGuid(resourceId).ToList();
                }

                var listdata = files.Select(x => new
                {
                    contractResourceFileGuid = x.ContractResourceFileGuid,
                    ResourceGuid = x.ResourceGuid,
                    UploadFileName = x.UploadFileName,
                    Description = x.Description,
                    FilePath = x.FilePath,
                    FileSize = x.FileSize,
                    Keys = x.Keys,
                    UpdatedBy = GetUserFullName(x.UpdatedBy),
                    UpdatedOn = x.UpdatedOn.ToString("MM/dd/yyyy")
                }).ToList();
                return Json(listdata);

            }
            catch (Exception ex)
            {
                return BadRequestFormatter.BadRequest(this, ex);
            }
        }

        [HttpGet]
        [Authorize]
        public ActionResult Add(Guid contractGuid, string formType)
        {
            try
            {
                var fileModel = new ContractFileViewModel();
                Guid contractResourceFileGuid = Guid.NewGuid();
                fileModel.ContractResourceFileGuid = contractResourceFileGuid;
                fileModel.ResourceGuid = contractGuid;
                fileModel.keys = formType;

                return PartialView(fileModel);
            }
            catch (Exception ex)
            {
                return BadRequestFormatter.BadRequest(this, ex);
            }
        }

        [HttpGet]
        [Authorize]
        public ActionResult Detail(Guid contractGuid, string formType)
        {
            var fileEntity = _contractRefactorService.GetFilesByContractGuid(contractGuid, formType);
            var model = _mapper.Map<ContractFileViewModel>(fileEntity);
            try
            {
                return PartialView(model);
            }
            catch (Exception ex)
            {
                return BadRequestFormatter.BadRequest(this, ex);
            }
        }

        [HttpGet]
        [Authorize]
        public ActionResult Edit(Guid contractGuid, string formType)
        {
            var fileEntity = _contractRefactorService.GetFilesByContractGuid(contractGuid, formType);
            var model = _mapper.Map<ContractFileViewModel>(fileEntity);
            try
            {
                return PartialView(model);
            }
            catch (Exception ex)
            {
                return BadRequestFormatter.BadRequest(this, ex);
            }
        }

        /// <summary>
        /// Get the list of Csv For the Grid list
        /// </summary>
        //[HttpGet]
        //[Authorize]
        //public IActionResult Get(Guid contractGuid, string formType)
        //{
        //    try
        //    {
        //        var fileEntity = _contractRefactorService.GetFilesByContractGuid(contractGuid, formType);
        //        var model = _mapper.Map<ContractFileViewModel>(fileEntity);
        //        var data = CsvValidationHelper.ChecksValidHeaderAndReadTheFile(model.UploadFileName, (Models.ViewModels.EnumGlobal.UploadMethodName)UploadMethodName.WorkBreakDownStructure);
        //        var listdata = data.Select(x => new
        //        {
        //            ContractGuid = contractGuid,
        //            WBSCode = x.WBSCode,
        //            Description = x.Description,
        //            Value = x.Value,
        //            ContractType = x.ContractType,
        //            InvoiceAtThisLevel = x.InvoiceAtThisLevel
        //        }).ToList();
        //        return Json(new { data = listdata });
        //    }
        //    catch (Exception ex)
        //    {
        //        ModelState.AddModelError(ex.ToString(), ex.Message);
        //        return BadRequest(ModelState);
        //    }
        //}

        [HttpPost]
        [Authorize]
        public IActionResult Add([FromForm]ContractFileViewModel fileModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                fileModel.CreatedOn = CurrentDateTimeHelper.GetCurrentDateTime();
                fileModel.UpdatedOn = CurrentDateTimeHelper.GetCurrentDateTime();
                fileModel.CreatedBy = UserHelper.CurrentUserGuid(HttpContext);
                fileModel.UpdatedBy = UserHelper.CurrentUserGuid(HttpContext);
                fileModel.IsActive = true;
                fileModel.IsDeleted = false;
                fileModel.IsCsv = true;
                // gets the contractnumber to save the file in the folder.
                var ContractNumber = _contractRefactorService.GetContractNumberById(fileModel.ResourceGuid);
                if (fileModel.FileToUpload != null || fileModel.FileToUpload.Length != 0)
                {
                    //checks whether the file extension is the correct one and the validates the fields if the file is Csv.
                    var isfileValid = _fileService.UploadFileTypeCheck(fileModel.FileToUpload);
                    //var filename = _fileService.FilePostWithCount($@"{documentRoot}/{ContractNumber}/WorkBreakdownStructure/", fileModel.FileToUpload);
                    if (!isfileValid)
                    {
                        fileModel.IsCsv = false;
                    }
                }
                //soft delete the previous uploaded files
                _contractRefactorService.DeleteContractFileByContractGuid(fileModel.ResourceGuid, fileModel.keys);

                var fileEntity = _mapper.Map<ContractResourceFile>(fileModel);
                _contractRefactorService.InsertContractFile(fileEntity);
                return Ok(new { status = ResponseStatus.success.ToString(), message = "Successfully Added !!" });
            }
            catch (Exception ex)
            {
                return BadRequestFormatter.BadRequest(this, ex);
            }
        }

        [HttpPost]
        [Authorize]
        public IActionResult Edit([FromForm]ContractFileViewModel fileModel)
        {
            try
            {
                fileModel.UpdatedOn = CurrentDateTimeHelper.GetCurrentDateTime();
                fileModel.UpdatedBy = UserHelper.CurrentUserGuid(HttpContext);
                fileModel.IsActive = true;
                fileModel.IsDeleted = false;
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var fileEntity = _mapper.Map<ContractResourceFile>(fileModel);
                _contractRefactorService.UpdateContractFile(fileEntity);
                return Ok(new { status = ResponseStatus.success.ToString(), message = "Successfully Edited !!" });
            }
            catch (Exception ex)
            {
                return BadRequestFormatter.BadRequest(this, ex);
            }
        }


        private String GetUserFullName(Guid userGuid)
        {
            var user = _userService.GetUserByUserGuid(userGuid);
            if (user == null)
                return "";
            var fullName = FormatHelper.FormatFullName(user.Firstname, "", user.Lastname);
            return fullName;
        }
    }
}