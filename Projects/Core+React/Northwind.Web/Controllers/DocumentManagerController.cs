using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Northwind.Core.Entities.ContractRefactor;
using Northwind.Core.Entities.DocumentMgmt;
using Northwind.Core.Interfaces.ContractRefactor;
using Northwind.Core.Interfaces.DocumentMgmt;
using Northwind.Web.Infrastructure.Authorization;
using Northwind.Web.Infrastructure.Helpers;
using Northwind.Web.Infrastructure.Models;
using Northwind.Web.Models;
using Northwind.Web.Models.ViewModels;
using Northwind.Web.Models.ViewModels.Contract;
using static Northwind.Core.Entities.EnumGlobal;

namespace Northwind.Web.Controllers
{
    [Authorize]
    public class DocumentManagerController : Controller
    {
        IConfiguration _configuration;
        string documentRoot;
        private readonly IDocumentManagementService _documentManagementService;
        private readonly IFolderStructureMasterService _folderStructureMasterService;
        private readonly IFolderStructureFolderService _folderStructureFolderService;
        private readonly IMapper _mapper;
        private readonly IContractsService _contractsService;
        public DocumentManagerController(IConfiguration configuration, IFolderStructureMasterService folderStructureMasterService, 
            IDocumentManagementService documentManagementService, IFolderStructureFolderService folderStructureFolderService, IMapper mapper, IContractsService contractsService)
        {
            _configuration = configuration;
            documentRoot = configuration.GetSection("Document").GetValue<string>("DocumentRoot");
            _documentManagementService = documentManagementService;
            _folderStructureFolderService = folderStructureFolderService;
            _folderStructureMasterService = folderStructureMasterService;
            _mapper = mapper;
            _contractsService = contractsService;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">RootPath of document folder</param>
        /// <returns></returns>
        [Secure(ResourceType.DocumentManager, ResourceActionPermission.View)]
        public IActionResult GetFolderTree(string id, string resourceType, Guid resourceId)
        {
            try
            {
                var UserGuid = UserHelper.CurrentUserGuid(HttpContext);
                var structureApplied = _documentManagementService.HasDefaultStructure(resourceType, resourceId);
                var contract = _contractsService.GetContractEntityByContractId(resourceId);
                if (!structureApplied && contract != null)
                {
                    var masterData = _folderStructureMasterService.GetActive("ESSWeb", resourceType);
                    if (masterData.Count() > 0)
                    {
                        var templateFolders = _folderStructureFolderService.GetFolderTree(masterData.SingleOrDefault().FolderStructureMasterGuid);
                        _documentManagementService.ManageDefaultStructure(id.Replace("/", ""),contract.ContractNumber, templateFolders, resourceType, resourceId, UserGuid);
                    }
                }
                //var folders = GetFolders(id, resourceId);
                var folders = _documentManagementService.GetFolderTree(resourceType, resourceId);
                return Ok(folders);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequestFormatter.BadRequest(this, ex);
            }
        }

        [Secure(ResourceType.DocumentManager, ResourceActionPermission.List)]
        public IActionResult GetFilesAndFolderTree(string id, string resourceType, Guid resourceId)
        {
            try
            {
                var UserGuid = UserHelper.CurrentUserGuid(HttpContext);
                var structureApplied = _documentManagementService.HasDefaultStructure(resourceType, resourceId);
                var contract = _contractsService.GetContractEntityByContractId(resourceId);
                if (!structureApplied && contract != null)
                {
                    var masterData = _folderStructureMasterService.GetActive("ESSWeb", resourceType);
                    if (masterData.Count() > 0)
                    {
                        var templateFolders = _folderStructureFolderService.GetFolderTree(masterData.SingleOrDefault().FolderStructureMasterGuid);
                        _documentManagementService.ManageDefaultStructure(id.Replace("/", ""), contract.ContractNumber,templateFolders, resourceType, resourceId, UserGuid);
                    }
                }
                //var folders = GetFolders(id, resourceId);
                IDocumentEntity folders = _documentManagementService.GetFilesAndFolderTree(resourceType, resourceId);
                return Ok(folders);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequestFormatter.BadRequest(this, ex);
            }
        }

        public IActionResult GetTemplateFolderTree(string resourceType)
        {
            var masterData = _folderStructureMasterService.GetActive("ESSWeb", resourceType);
            if (masterData.Count() > 0)
            {
                return Json(_folderStructureFolderService.GetFolderTree(masterData.SingleOrDefault().FolderStructureMasterGuid));
            }
            return null;
        }

        public IActionResult GetFilesAndFolders(string id, string resourceType, Guid fileId)
        {
            try
            {
                if (fileId != new Guid())
                {
                    var filesandFolders = _documentManagementService.GetFilesAndFoldersByParentId(resourceType, fileId);
                    return Ok(filesandFolders);
                }
                else
                {

                    return Ok(new ContractResourceFile());
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return BadRequestFormatter.BadRequest(this, e);
            }
        }

        [Secure(ResourceType.DocumentManager, ResourceActionPermission.Manage)]
        [HttpGet]
        public IActionResult Operation(string id, string id1, string resourceType, Guid resourceId, Guid parentId, Guid fileId, string id2 = "")
        {
            id = id.Replace("-amp;", "&");
            if (id1.ToUpper() == "CreateFolder".ToUpper())
            {
                return CreateFolder(id, resourceType, resourceId, parentId);
            }
            else if (id1.ToUpper() == "DeleteFolder".ToUpper())
            {
                return DeleteFolder(id, resourceType, fileId);
            }
            else if (id1.ToUpper() == "DeleteFile".ToUpper())
            {
                return DeleteFile(id, resourceType, fileId);
            }
            else if (id1.ToUpper() == "RenameFolder".ToUpper())
            {
                return RenameFolder(id, fileId);
            }
            else if (id1.ToUpper() == "RenameFile".ToUpper())
            {
                return RenameFile(id, resourceType, fileId);
            }
            else if (id1.ToUpper() == "SearchFiles".ToUpper())
            {
                return SearchFilesAndFolders(id, resourceType, id2);
            }
            else if (id1.ToUpper() == "Download".ToUpper())
            {
                return DownloadFile(id);
            }
            return null;
        }

        [Secure(ResourceType.DocumentManager, ResourceActionPermission.Manage)]
        [HttpPost]
        public IActionResult Operation([FromBody] DocumentOperation model)
        {
            if (model.Operation.ToUpper() == "CreateFolder".ToUpper())
            {
                return CreateFolder(model);
            }
            else if (model.Operation.ToUpper() == "DeleteFolder".ToUpper())
            {
                return DeleteFolder(model);
            }
            else if (model.Operation.ToUpper() == "DeleteFile".ToUpper())
            {
                return DeleteFile(model);
            }
            else if (model.Operation.ToUpper() == "RenameFolder".ToUpper())
            {
                return RenameFolder(model);
            }
            else if (model.Operation.ToUpper() == "FileCutPaste".ToUpper())
            {
                return FileCutPaste(model);
            }
            else if (model.Operation.ToUpper() == "FileCopyPaste".ToUpper())
            {
                return FileCopyPaste(model);
            }
            else if (model.Operation.ToUpper() == "RenameFile".ToUpper())
            {
                return RenameFile(model);
            }
            else if (model.Operation.ToUpper() == "FolderCopyPaste".ToUpper())
            {
                return FolderCopyPaste(model);
            }
            else if (model.Operation.ToUpper() == "FolderCutPaste".ToUpper())
            {
                return FolderCutPaste(model);
            }
            return null;
        }

        [HttpGet]
        public ActionResult CreateFolder(string id, string resourceType, Guid resourceId, Guid parentId)
        {
            var createFolder = new DocumentOperation();
            createFolder.Path = id;
            createFolder.ResourceId = resourceId;
            createFolder.ResourceType = resourceType;
            createFolder.ParentId = parentId;
            createFolder.Operation = "CreateFolder";
            return PartialView("CreateFolder", createFolder);
        }

        [HttpPost]
        public IActionResult CreateFolder([FromBody] DocumentOperation model)
        {
            try
            {
                var path = model.Path;
                if (!path.EndsWith("/"))
                    path += "/";
                if (!Directory.Exists(documentRoot + "\\" + path + model.Name))
                {
                    var info = Directory.CreateDirectory(documentRoot + "\\" + path + model.Name);
                    var doc = CreateFolderObject(model);
                    _documentManagementService.CreateFileOrFolder(doc);
                }
                else
                {
                    throw new Exception("Directory already exists");
                }
                return Ok(new { status = true });
            }
            catch (Exception ex)
            {
                var path = Path.Combine(documentRoot, model.Path, model.Name);
                if (!Directory.Exists(path))
                {
                    try
                    {
                        Directory.Delete(path);
                    }
                    catch (Exception)
                    {
                    }
                }
                ModelState.AddModelError("", ex.Message);
                return BadRequestFormatter.BadRequest(this, ex);
            }
        }

        [HttpGet]
        public ActionResult RenameFolder(string id, Guid fileId)
        {
            var createFolder = new DocumentOperation();
            createFolder.Path = id;
            try
            {
                var path = documentRoot + createFolder.Path.Replace("/", "\\");
                if (Directory.Exists(path))
                {
                    var doc = _documentManagementService.GetFileOrFolder(fileId);
                    if (doc == null)
                    {
                        throw new Exception("Unable to find folder");
                    }
                    var finfo = new FileInfo(path);
                    createFolder.Name = finfo.Name;
                    createFolder.Path = doc.FilePath;
                    createFolder.Description = doc.Description;
                    createFolder.Operation = "RenameFolder";
                    createFolder.FileId = fileId;
                    return PartialView("RenameFolder", createFolder);
                }
                else
                {
                    throw new Exception("Unable to find folder");
                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return PartialView();
            }
        }

        [HttpPost]
        public ActionResult RenameFolder([FromBody] DocumentOperation model)
        {
            try
            {
                var path = documentRoot + model.Path.Replace("/", "\\");
                var doc = _documentManagementService.GetFileOrFolder(model.FileId);
                if (Directory.Exists(path) && doc.IsReadOnly == false)
                {
                    var src = path;
                    var dest = Directory.GetParent(src).FullName + "\\" + model.Name;
                    if (src.ToLower() != dest.ToLower())
                    {
                        Directory.Move(src, dest);
                    }


                    if (doc != null)
                    {
                        var finfo = new FileInfo(path);
                        var UserGuid = UserHelper.CurrentUserGuid(HttpContext);
                        doc.FilePath = finfo.DirectoryName.Replace(documentRoot, "") + "\\" + model.Name;
                        doc.Description = model.Description;
                        doc.UploadFileName = model.Name;
                        doc.UploadUniqueFileName = model.Name;
                        doc.UpdatedBy = UserGuid;
                        doc.UpdatedOn = DateTime.UtcNow;

                    }
                    _documentManagementService.UpdateFileOrFolder(doc);
                    return Ok(new { status = true });
                }
                else
                {
                    throw new Exception("Unable to find folder");
                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequestFormatter.BadRequest(this, ex);
            }
        }

        private IDocumentEntity CreateFolderObject(DocumentOperation model)
        {
            var UserGuid = UserHelper.CurrentUserGuid(HttpContext);
            var doc = new DocumentEntity
            {
                CreatedBy = UserGuid,
                CreatedOn = DateTime.UtcNow,
                Description = model.Description,
                FilePath = Path.Combine(model.Path, model.Name),
                FileSize = "0",
                IsActive = true,
                IsCsv = false,
                IsDeleted = false,
                IsFile = false,
                IsReadOnly = false,
                ResourceType = model.ResourceType,
                Keys = "",
                MasterFolderGuid = Guid.Empty,
                MasterStructureGuid = Guid.Empty,
                MimeType = "Folder",
                ParentId = model.ParentId,
                ResourceGuid = model.ResourceId,
                Type = "Folder",
                UpdatedBy = UserGuid,
                UpdatedOn = DateTime.UtcNow,
                UploadFileName = model.Name,
                UploadUniqueFileName = model.Name,
                ContractResourceFileGuid = Guid.NewGuid()
            };
            return doc;
        }

        [HttpPost]
        public ActionResult DeleteFile([FromBody] DocumentOperation model)
        {
            try
            {
                var file = _documentManagementService.GetFileOrFolder(model.FileId);
                if (file != null && file.IsReadOnly == true)
                {
                    throw new Exception("System file cannot be deleted.");
                }
                _documentManagementService.DeleteFilesAndFoldersByGuid(model.ResourceType, model.FileId);
                var path = documentRoot + "\\" + model.Path;
                System.IO.File.Move(path, path + "_" + model.FileId.ToString());
                return Ok(new { status = true });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequestFormatter.BadRequest(this, ex);
            }
        }

        [HttpPost]
        public ActionResult DeleteFolder([FromBody] DocumentOperation model)
        {
            try
            {
                var doc = _documentManagementService.GetFilesAndFoldersByGuid(model.ResourceType, model.FileId);
                if (doc.Count() > 0)
                {
                    if (doc.SingleOrDefault().IsReadOnly)
                    {
                        throw new Exception("You are not allowed to delete the template folders.");
                    }
                    Directory.Move(documentRoot + "\\" + model.Path, documentRoot + "\\" + model.Path + "_" + doc.Single().ContractResourceFileGuid.ToString());
                    _documentManagementService.DeleteFilesAndFoldersByGuid(model.ResourceType, model.FileId);
                }

                return Ok(new { status = true });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequestFormatter.BadRequest(this, ex);
            }
        }

        [HttpGet]
        public ActionResult DeleteFolder(string id, string resourceType, Guid fileId)
        {
            var doc = _documentManagementService.GetFilesAndFoldersByGuid(resourceType, fileId);
            try
            {
                if (doc.Count() == 0)
                {
                    throw new Exception("Unable to find folder to delete.");
                }
                else
                {
                    if (doc.SingleOrDefault().IsReadOnly)
                    {
                        throw new Exception("You are not allowed to delete the template folders");
                    }
                }
                var deleteFolder = new DocumentOperation();
                deleteFolder.Path = id;
                var path = documentRoot + "\\" + deleteFolder.Path;
                var finfo = new FileInfo(path);
                deleteFolder.Name = finfo.Name;
                deleteFolder.Description = doc.SingleOrDefault().Description;
                deleteFolder.Operation = "DeleteFolder";
                return PartialView("DeleteFolder", deleteFolder);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequestFormatter.BadRequest(this, ex);
            }

        }

        [HttpPost]
        public IActionResult FolderCopyPaste([FromBody] DocumentOperation model)
        {
            try
            {
                var path = model.Path;
                var srcPath = model.Source;
                if (!path.EndsWith("/"))
                    path += "/";

                var finfo = new FileInfo(srcPath);
                var folderName = finfo.Name;

                if (Directory.Exists(documentRoot + path + folderName))
                {
                    folderName = GetNewDirectoryPath(documentRoot + path + folderName, 0);
                }
                path = path + folderName;

                //if (model.FileId.Equals(model.DestinationGuid))
                //{
                //    throw new Exception("Source and destination folder cannot be same.");
                //}

                var source = _documentManagementService.GetFileOrFolder(model.FileId);
                var destination = _documentManagementService.GetFileOrFolder(model.DestinationGuid);

                CopyFolder(source, destination);

                return Ok(new { status = true });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message.Replace(documentRoot, ""));
                return BadRequestFormatter.BadRequest(this, ex);
            }
        }

        [HttpPost]
        public IActionResult FolderCutPaste([FromBody] DocumentOperation model)
        {
            try
            {
                var path = model.Path;
                var srcPath = model.Source;
                if (!path.EndsWith("/"))
                    path += "/";

                var finfo = new FileInfo(srcPath);
                var folderName = finfo.Name;

                if (Directory.Exists(documentRoot + path + folderName))
                {
                    folderName = GetNewDirectoryPath(documentRoot + path + folderName, 0);
                }
                path = path + folderName;

                var source = _documentManagementService.GetFileOrFolder(model.FileId);
                var destination = _documentManagementService.GetFileOrFolder(model.DestinationGuid);

                Directory.Move(documentRoot + source.FilePath, documentRoot + destination.FilePath + "/" + source.UploadFileName);
                source.ParentId = destination.ContractResourceFileGuid;
                source.FilePath = destination.FilePath + "/" + source.UploadFileName;
                _documentManagementService.UpdateFileOrFolder(source);
                ManageChildFolderAndFilePath(source);
                return Ok(new { status = true });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequestFormatter.BadRequest(this, ex);
            }
        }

        private void ManageChildFolderAndFilePath(IDocumentEntity source)
        {
            var children = _documentManagementService.GetFilesAndFoldersByParentId(source.ResourceType, source.ContractResourceFileGuid);
            foreach(var item in children)
            {
                item.FilePath = source.FilePath + "/" + item.UploadFileName;
                _documentManagementService.UpdateFileOrFolder(item);
                ManageChildFolderAndFilePath(item);
            }
        }

        [HttpGet]
        public ActionResult DeleteFile(string id, string resourceType, Guid fileId)
        {
            var doc = _documentManagementService.GetFileOrFolder(fileId);
            var DeleteFile = new DocumentOperation();
            DeleteFile.Path = id;
            var path = documentRoot + "\\" + DeleteFile.Path;
            var finfo = new FileInfo(path);
            DeleteFile.Name = finfo.Name;
            DeleteFile.ResourceType = resourceType;
            DeleteFile.Description = doc.Description;
            DeleteFile.Operation = "DeleteFile";
            return PartialView("DeleteFile", DeleteFile);
        }

        private string GetNewDirectoryPath(string filePath, int index)
        {
            var finfo = new FileInfo(filePath);
            var name = finfo.Name;
            var newPath = "";
            if (index > 0)
            {
                newPath = name + "_" + index.ToString();
            }
            else
            {
                newPath = name;
            }

            finfo = new FileInfo(Directory.GetParent(filePath).FullName + "\\" + newPath);
            if (Directory.Exists(finfo.FullName))
                return GetNewDirectoryPath(filePath, ++index);
            return finfo.FullName;
        }

        private void CopyFolder(IDocumentEntity source, IDocumentEntity destination)
        {
            var docPath = GetNewDirectoryPath(documentRoot + destination.FilePath + "/" + source.UploadFileName, 0);
            if (source.IsFile)
            {
                System.IO.File.Copy(documentRoot + source.FilePath, docPath, true);
            }
            else
            {
                Directory.CreateDirectory(docPath);
            }

            var fInfo = new FileInfo(docPath);

            var folder = new DocumentEntity
            {
                ContractResourceFileGuid = Guid.NewGuid(),
                CreatedBy = source.CreatedBy,
                CreatedOn = DateTime.UtcNow,
                Description = source.Description,
                FilePath = docPath.Replace(documentRoot, ""),
                FileSize = "0",
                IsActive = source.IsActive,
                IsCsv = source.IsCsv,
                IsDeleted = source.IsDeleted,
                IsFile = source.IsFile,
                IsReadOnly = false,
                Keys = source.Keys,
                MasterFolderGuid = source.MasterFolderGuid,
                MasterStructureGuid = source.MasterStructureGuid,
                MimeType = source.MimeType,
                ParentId = destination.ContractResourceFileGuid,
                ResourceGuid = source.ResourceGuid,
                ResourceType = destination.ResourceType,
                Type = source.Type,
                UpdatedBy = source.UpdatedBy,
                UpdatedOn = source.UpdatedOn,
                UploadFileName = fInfo.Name,
                UploadUniqueFileName = fInfo.Name
            };
            _documentManagementService.CreateFileOrFolder(folder);
            var childFolders = _documentManagementService.GetFilesAndFoldersByParentId(source.ResourceType, source.ContractResourceFileGuid);
            foreach (var c in childFolders)
            {
                if (!c.ContractResourceFileGuid.Equals(folder.ContractResourceFileGuid))
                {
                    CopyFolder(c, folder);
                }                
            }
        }

        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> Upload(string id, string resourceType, Guid parentId, [FromForm]IEnumerable<IFormFile> files)
        {
            try
            {
                var UserGuid = UserHelper.CurrentUserGuid(HttpContext);
                var doc = _documentManagementService.GetFileOrFolder(parentId);
                var path = documentRoot + doc.FilePath;
                if (!path.EndsWith("/"))
                    path += "/";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                foreach (var file in files)
                {
                    var oFileInfo = new FileInfo(file.FileName);
                    var filePath = path + oFileInfo.Name;
                    var savingFilePath = filePath;
                    if (System.IO.File.Exists(filePath))
                    {
                        savingFilePath = GetNewFilePath(filePath, 0);
                    }
                    FileStream fileStream = System.IO.File.Create(savingFilePath);
                    oFileInfo = new FileInfo(savingFilePath);
                    await file.CopyToAsync(fileStream);
                    fileStream.Close();
                    var objFile = new DocumentEntity
                    {
                        ContractResourceFileGuid = Guid.NewGuid(),
                        CreatedBy = UserGuid,
                        CreatedOn = DateTime.UtcNow,
                        Description = "",
                        FilePath = savingFilePath.Replace(documentRoot, ""),
                        FileSize = file.Length.ToString(),
                        IsActive = true,
                        IsCsv = file.ContentType.ToLower().Contains("csv"),
                        IsDeleted = false,
                        IsFile = true,
                        IsReadOnly = false,
                        Keys = doc.Keys,
                        ResourceType = resourceType,
                        MimeType = file.ContentType,
                        ParentId = doc.ContractResourceFileGuid,
                        ResourceGuid = doc.ResourceGuid,
                        UploadFileName = oFileInfo.Name,
                        UploadUniqueFileName = oFileInfo.Name,
                        UpdatedBy = UserGuid,
                        UpdatedOn = DateTime.UtcNow
                    };
                    _documentManagementService.CreateFileOrFolder(objFile);
                }
                return Ok(new { status = true });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequestFormatter.BadRequest(this, ex);
            }

        }

        private string GetNewFilePath(string filePath, int index)
        {
            var finfo = new FileInfo(filePath);
            var name = finfo.Name;
            var ext = finfo.Extension;
            name = name.Replace(ext, "");
            name += "_" + index.ToString();
            var newPath = finfo.Directory.FullName + "\\" + name + ext;
            if (System.IO.File.Exists(newPath))
                return GetNewFilePath(filePath, ++index);
            return newPath;
        }

        public IActionResult DownloadFile(string id)
        {
            var file = new FileInfo(documentRoot + id);
            var fileProvider = new FileExtensionContentTypeProvider();
            if (!fileProvider.TryGetContentType(file.Extension, out string contentType))
            {
                throw new ArgumentOutOfRangeException($"Unable to find Content Type for file name {file.Extension}.");
            }

            return File(new FileStream(documentRoot + id, FileMode.Open, FileAccess.Read), contentType, file.Name);
        }

        [HttpPost]
        public IActionResult FileCopyPaste([FromBody] DocumentOperation model)
        {
            try
            {
                var path = model.Path;
                var srcPath = model.Source;
                if (!path.EndsWith("/"))
                    path += "/";

                var fInfo = new FileInfo(documentRoot + srcPath);
                var file = _documentManagementService.GetFileOrFolder(model.FileId);
                if (file != null)
                {

                    if (fInfo.Exists)
                    {
                        if (Directory.Exists(documentRoot + path))
                        {
                            var dest = documentRoot + path + fInfo.Name;
                            if (System.IO.File.Exists(dest))
                            {
                                dest = GetNewFilePath(dest, 0);
                            }
                            var destFile = new FileInfo(dest);
                            file.ContractResourceFileGuid = Guid.NewGuid();
                            file.ParentId = model.DestinationGuid;
                            file.FilePath = destFile.FullName.Replace(documentRoot, "");
                            file.UploadFileName = destFile.Name;
                            file.UploadUniqueFileName = destFile.Name;
                            _documentManagementService.CreateFileOrFolder(file);
                            fInfo.CopyTo(dest);

                        }
                        else
                        {
                            throw new Exception("Unable to find destination folder");
                        }
                    }
                    else
                    {
                        throw new Exception("Unable to find source file.");
                    }
                }
                else
                {
                    throw new Exception("Unable to find source file.");
                }
                return Ok(new { status = true });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequestFormatter.BadRequest(this, ex);
            }
        }

        [HttpPost]
        public IActionResult FileCutPaste([FromBody] DocumentOperation model)
        {
            try
            {
                var path = model.Path;
                var srcPath = model.Source;
                if (!path.EndsWith("/"))
                    path += "/";

                var fInfo = new FileInfo(documentRoot + srcPath);
                var file = _documentManagementService.GetFileOrFolder(model.FileId);
                if (file != null)
                {
                    if (fInfo.Exists)
                    {
                        if (Directory.Exists(documentRoot + path))
                        {
                            var dest = documentRoot + path + fInfo.Name;
                            if (System.IO.File.Exists(dest))
                            {
                                dest = GetNewFilePath(dest, 0);
                            }
                            fInfo.MoveTo(dest);
                            var destFile = new FileInfo(dest);
                            file.ParentId = model.DestinationGuid;
                            file.FilePath = destFile.FullName.Replace(documentRoot, "");
                            file.UploadFileName = destFile.Name;
                            file.UploadUniqueFileName = destFile.Name;
                            _documentManagementService.UpdateFileOrFolder(file);
                        }
                        else
                        {
                            throw new Exception("Unable to find destination folder");
                        }
                    }
                    else
                    {
                        throw new Exception("Unable to find source file.");
                    }
                    return Ok(new { status = true });
                }
                else
                {
                    ModelState.AddModelError("", "Unable to find file to delete.");
                    return BadRequestFormatter.BadRequest(this, "Unable to find file to delete.");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequestFormatter.BadRequest(this, ex);
            }

        }

        [HttpGet]
        public ActionResult RenameFile(string id, string resourceType, Guid fileGuid)
        {
            var createFolder = new DocumentOperation();
            createFolder.Path = id;
            try
            {
                var doc = _documentManagementService.GetFileOrFolder(fileGuid);
                if (System.IO.File.Exists(documentRoot + "\\" + doc.FilePath) && doc != null)
                {
                    var finfo = new FileInfo(documentRoot + "\\" + createFolder.Path);
                    createFolder.Name = finfo.Name;
                    createFolder.Path = id;
                    createFolder.FileId = doc.ContractResourceFileGuid;
                    createFolder.Operation = "RenameFile";
                    return PartialView("RenameFile", createFolder);
                }
                else
                {
                    throw new Exception("Unable to find file");
                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return PartialView();
            }
        }

        [HttpPost]
        public ActionResult RenameFile([FromBody] DocumentOperation model)
        {
            try
            {
                var file = _documentManagementService.GetFileOrFolder(model.FileId);
                if (file != null)
                {
                    var fInfo = new FileInfo(documentRoot + "\\" + file.FilePath);
                    if (fInfo.Exists)
                    {
                        var dest = fInfo.Directory.FullName + "\\" + model.Name;
                        if (fInfo.FullName != dest)
                        {
                            if (System.IO.File.Exists(dest))
                            {
                                dest = GetNewFilePath(dest, 0);
                            }
                        }
                        fInfo.MoveTo(dest);

                        if (file != null)
                        {
                            fInfo = new FileInfo(dest);
                            file.UploadFileName = fInfo.Name;
                            file.FilePath = dest.Replace(documentRoot, "");
                            _documentManagementService.UpdateFileOrFolder(file);
                        }
                        return Ok(new { status = true });
                    }
                    else
                    {
                        throw new Exception("Unable to find file.");
                    }
                }
                else
                {
                    throw new Exception("Unable to find file.");
                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequestFormatter.BadRequest(this, ex);
            }
        }

        public IActionResult SearchFilesAndFolders(string id, string resourceType, String searchString)
        {
            try
            {
                var result = _documentManagementService.SearchFilesAndFolders(resourceType, id, searchString);
                return Ok(result);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequestFormatter.BadRequest(this, ex);
            }
        }


        /*
        public IActionResult SearchFilesAndFolders(string id, string id1)
        {
            try
            {
                var result = new List<Folder>();
                var searchPath = documentRoot + id;
                string[] files = Directory.GetFiles(searchPath, $"*{id1}*", SearchOption.AllDirectories);
                foreach (var file in files)
                {
                    var finfo = new FileInfo(file);
                    result.Add(new Folder
                    {
                        Name = finfo.Name,
                        RelativePath = file.Replace(documentRoot, "").Replace("\\", "/"),
                        Type = string.IsNullOrEmpty(finfo.Extension) ? "folder" : "file"
                    });
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequestFormatter.BadRequest(this, ModelState);
            }
        }

        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">Abolute Path</param>
        /// <returns></returns>
        public IActionResult GetFilesAndFolders(string id,Guid fileId)
        {
            try
            {
                if (fileId != new Guid())
                {
                    var filesandFolders = _contractRefactorService.GetFilesAndFoldersByParentId(fileId);
                    return Ok(filesandFolders);
                }
                else
                {
                    
                    return Ok(new ContractResourceFile());
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return BadRequest(_configuration.GetSection("ExceptionErrorMessage").Value);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">It is the path of folder</param>
        /// <param name="id1">It is the operation</param>
        /// <returns></returns>
        

        

        

        

        

        [HttpPost]
        public IActionResult FileCopyPaste([FromBody] DocumentOperation model)
        {
            try
            {
                var path = model.Path;
                var srcPath = model.Source;
                if (!path.EndsWith("/"))
                    path += "/";

                var fInfo = new FileInfo(documentRoot + srcPath);
                if (fInfo.Exists)
                {
                    if (Directory.Exists(documentRoot + path))
                    {
                        var dest = documentRoot + path + fInfo.Name;
                        if (System.IO.File.Exists(dest))
                        {
                            dest = GetNewFilePath(dest, 0);
                        }
                        fInfo.CopyTo(dest);
                    }
                    else
                    {
                        throw new Exception("Unable to find destination folder");
                    }
                }
                else
                {
                    throw new Exception("Unable to find source file.");
                }
                return Ok(new { status = true });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequestFormatter.BadRequest(this, ModelState);
            }
        }

        
        

        

        

        

        [HttpGet]
        public ActionResult CreateFolder(string id,Guid resourceId,Guid parentId)
        {
            var createFolder = new DocumentOperation();
            createFolder.Path = id;
            createFolder.ResourceId = resourceId;
            createFolder.ParentId = parentId;
            createFolder.Operation = "CreateFolder";
            return PartialView("CreateFolder", createFolder);
        }

        [HttpPost]
        public IActionResult CreateFolder([FromBody] DocumentOperation model)
        {
            try
            {
                var path = model.Path;
                if (!path.EndsWith("/"))
                    path += "/";
                if (!Directory.Exists(documentRoot + "\\" + path + model.Name))
                {
                    var info = Directory.CreateDirectory(documentRoot + "\\" + path + model.Name);
                    UploadFileFolderInDB(model.Path, model.ResourceId, model.Name, model.ParentId);
                }
                else
                {
                    throw new Exception("Directory already exists");
                }
                return Ok(new { status = true });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequestFormatter.BadRequest(this, ModelState);
            }
        }


        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> Upload(string id, [FromForm]List<IFormFile> files)
        {
            try
            {
                var path = documentRoot + id;
                if (!path.EndsWith("/"))
                    path += "/";
                if (!Directory.Exists(path))
                {
                    throw new Exception("Unable to find folder to upload.");
                }
                foreach (var file in files)
                {
                    var oFileInfo = new FileInfo(file.FileName);
                    var filePath = path + oFileInfo.Name;
                    var savingFilePath = filePath;
                    if (System.IO.File.Exists(filePath))
                    {
                        savingFilePath = GetNewFilePath(filePath, 0);
                    }
                    FileStream fileStream = System.IO.File.Create(savingFilePath);
                    await file.CopyToAsync(fileStream);
                    fileStream.Close();
                }
                return Ok(new { status = true });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequestFormatter.BadRequest(this, ModelState);
            }
        }

        public void CopyFolderToDatabase(string sourceFolder, string destFolder,Guid sourceFileId,Guid destinationFileId)
        {
            
            ContractResourceFile contractResourceFile = _contractRefactorService.GetFilesByContractFileGuid(sourceFileId);
            var id =   InsertData(contractResourceFile, destFolder, destinationFileId);
            CopySubFolders(contractResourceFile, destFolder, destinationFileId);
            IEnumerable<ContractResourceFile> contractResourceFiles = _contractRefactorService.GetFilesAndFoldersByParentId(sourceFileId);
            foreach (var data in contractResourceFiles)
            {
                InsertData(data, destFolder, id);
            }

        }
        private Guid CopySubFolders(ContractResourceFile data, string filePath, Guid parentId)
        {
            return new Guid();
        }
        private Guid InsertData(ContractResourceFile data , string filePath,Guid parentId)
        {
            ContractFileViewModel contractFileViewModel = new ContractFileViewModel();
            contractFileViewModel.ContractResourceFileGuid = Guid.NewGuid();
            contractFileViewModel.ResourceGuid = data.ResourceGuid;
            contractFileViewModel.CreatedOn = CurrentDateTimeHelper.GetCurrentDateTime();
            contractFileViewModel.UpdatedOn = CurrentDateTimeHelper.GetCurrentDateTime();
            contractFileViewModel.CreatedBy = UserHelper.CurrentUserGuid(HttpContext);
            contractFileViewModel.UpdatedBy = UserHelper.CurrentUserGuid(HttpContext);
            contractFileViewModel.Description = "Folder Description";
            contractFileViewModel.MimeType = null;
            contractFileViewModel.keys = "Contract";
            contractFileViewModel.UploadFileName = data.UploadFileName;
            contractFileViewModel.UploadUniqueFileName = data.UploadUniqueFileName;
            contractFileViewModel.IsActive = true;
            contractFileViewModel.IsDeleted = false;
            contractFileViewModel.FilePath = filePath;
            contractFileViewModel.FileSize = null;
            contractFileViewModel.IsReadonly = false;
            contractFileViewModel.Isfile = false;
            contractFileViewModel.ParentId = parentId;
            var fileEntity = _mapper.Map<ContractResourceFile>(contractFileViewModel);
             _contractRefactorService.InsertContractFile(fileEntity);
            return contractFileViewModel.ContractResourceFileGuid;
        }
        private List<ContractResourceFile> GetFolders(string parentFolder,Guid resourceId)
        {
            string dir = documentRoot + parentFolder;
            var listFolders = CheckParentFolder(resourceId);
            if (!listFolders.Any())
            {
                if (UploadFileFolderInDB(parentFolder, resourceId))
                {
                    Directory.CreateDirectory(dir);
                     listFolders = CheckParentFolder(resourceId);
                }
                else
                {
                    return new List<ContractResourceFile>();
                }
            }
           
         return GetFoldersStructure(listFolders, Guid.Empty);
           

        }
        public List<ContractResourceFile> GetFoldersStructure(IEnumerable<ContractResourceFile> folders, Guid parentId)
        {
            List<ContractResourceFile> recursiveObjects = new List<ContractResourceFile>();
            foreach (var item in folders.Where(x => x.ParentId.Equals(parentId) && x.IsFile == false))
            {
                ContractResourceFile file = new ContractResourceFile();
                file.UploadFileName = item.UploadFileName;
                file.FilePath = item.FilePath;
                file.Type = item.IsFile ? "File" : "Folder";
                file.ContractResourceFileGuid = item.ContractResourceFileGuid;
                file.Subfolders = GetFoldersStructure(folders, item.ContractResourceFileGuid);
                recursiveObjects.Add(file);
            }
            return recursiveObjects;

        }

        public bool UploadFileFolderInDB(string uploadPath,Guid resourceId,string FileName="",Guid? parentId=null)
        {
            ContractFileViewModel contractFileViewModel = new ContractFileViewModel();
            contractFileViewModel.ContractResourceFileGuid = Guid.NewGuid();
            contractFileViewModel.ResourceGuid = resourceId;
            contractFileViewModel.CreatedOn = CurrentDateTimeHelper.GetCurrentDateTime();
            contractFileViewModel.UpdatedOn = CurrentDateTimeHelper.GetCurrentDateTime();
            contractFileViewModel.CreatedBy = UserHelper.CurrentUserGuid(HttpContext);
            contractFileViewModel.UpdatedBy = UserHelper.CurrentUserGuid(HttpContext);
            contractFileViewModel.Description = "Folder Description";
            contractFileViewModel.MimeType = null;
            contractFileViewModel.ResourceGuid = resourceId;
            contractFileViewModel.keys = "Contract";
            contractFileViewModel.UploadFileName = string.IsNullOrEmpty(FileName) ? uploadPath.Replace("/","") : FileName;
            contractFileViewModel.UploadUniqueFileName = uploadPath;
            contractFileViewModel.IsActive = true;
            contractFileViewModel.IsDeleted = false;
            contractFileViewModel.FilePath = uploadPath+"/"+FileName ;
            contractFileViewModel.FileSize = null;
            contractFileViewModel.IsReadonly = false;
            contractFileViewModel.Isfile = false;
            contractFileViewModel.ParentId = parentId;
            var fileEntity = _mapper.Map<ContractResourceFile>(contractFileViewModel);
          return  _contractRefactorService.InsertContractFile(fileEntity);

        }

        public bool DeleteFolder(Guid FileId)
        {
                return _contractRefactorService.DeleteContractFolder(FileId);

        }
        
        private IEnumerable<ContractResourceFile> CheckParentFolder(Guid resourceId)
        {
            return _contractRefactorService.GetFilesByContractResourceGuid(resourceId);
        }

        public bool RenameDBFolder(Guid FileId,string FolderName)
        {
            return _contractRefactorService.RenameContractFolder(FileId,FolderName);

        }

        
        


        
        */

    }

}
