using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using Northwind.Core.Interfaces.ContractRefactor;
using Northwind.Core.Services;
using Northwind.Web.Models;
using Northwind.Web.Models.ViewModels.Project;

namespace Northwind.Web.Controllers
{
    public class ProjectModificationController : Controller
    {
        private readonly IProjectModificationService _projectModificationService;
        //        private readonly IProjectService _projectService;
        private readonly IContractsService _contractService;
        private readonly IConfiguration _configuration;
        private readonly IFileService _fileService;
        private readonly IContractModificationService _contractModificationService;
        private readonly IMapper _mapper;
        public ProjectModificationController(
                                             //                                             IProjectService projectService, 
                                             IProjectModificationService projectModificationService,
                                             IConfiguration configuration,
                                             IFileService fileService,
                                             IContractsService contractService,
                                             IContractModificationService contractModificationService,
                                             IMapper mapper)
        {
            _projectModificationService = projectModificationService;
            //            _projectService = projectService;
            _contractService = contractService;
            _configuration = configuration;
            _fileService = fileService;
            _contractModificationService = contractModificationService;
            _mapper = mapper;
        }
        // GET: Project Modification
        public ActionResult Index(Guid ProjectGuid, string SearchValue)
        {
            var projectModificationModel = new ProjectModificationViewModel();
            projectModificationModel.SearchValue = SearchValue;
            projectModificationModel.ProjectGuid = ProjectGuid;
            return View(projectModificationModel);
        }
        public IActionResult Get(Guid ProjectGuid, string SearchValue, int pageSize, int skip, string sortField, string sortDirection)
        {
            try
            {
                var projectModificationEntityList = _contractModificationService.GetAll(ProjectGuid,true, SearchValue, pageSize, skip, sortField, sortDirection);
                var projectViewModels = _mapper.Map<IEnumerable<ProjectModificationViewModel>>(projectModificationEntityList);
                var totalRecordCount = _projectModificationService.TotalRecord(ProjectGuid);

                var data = projectViewModels.Select(x => new
                {
                    ProjectModificationGuid = x.ProjectModificationGuid,
                    ProjectNumber = x.ProjectNumber,
                    ModificationNumber = x.ModificationNumber,
                    ProjectTitle = x.ProjectTitle,
                    ModificationTitle = x.ModificationTitle,
                    EffectiveDate = x.EffectiveDate?.ToString("MM/dd/yyyy"),
                    EnteredDate = x.EnteredDate?.ToString("MM/dd/yyyy"),
                    PopStart = x.POPStart?.ToString("MM/dd/yyyy"),
                    PopEnd = x.POPEnd?.ToString("MM/dd/yyyy"),
                    AwardAmount = x.AwardAmount,
                    IsActiveStatus = x.IsActive == true ? EnumGlobal.ActiveStatus.Active : EnumGlobal.ActiveStatus.Inactive,
                    UpdatedOn = x.UpdatedOn
                }).ToList();

                return Json(new { total = totalRecordCount, data = data });
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return BadRequest(_configuration.GetSection("ExceptionErrorMessage").Value);
            }
        }

        #region  Crud

        [HttpGet]
        public ActionResult Add(Guid projectGuid)
        {
            ProjectModificationViewModel projectModificationModel = new ProjectModificationViewModel();
            try
            {
                projectModificationModel.ProjectGuid = projectGuid;
                projectModificationModel.ContractGuid = projectGuid;
                projectModificationModel.ContractNumber =
                    _contractService.GetDetailsForProjectByContractId(projectModificationModel.ContractGuid ?? Guid.Empty).ContractNumber;
                projectModificationModel.ProjectNumber =
                   _contractService.GetDetailById(projectGuid).BasicContractInfo.ProjectNumber;
                return PartialView(projectModificationModel);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return View(projectModificationModel);
            }
        }

        [HttpPost]
        public IActionResult Add([FromForm]ProjectModificationViewModel projectModificationModel)
        {
            try
            {
                List<string> filePath = new List<string>();

                //check for duplicate modification number..
                var isExistModificationNumber =
                    _projectModificationService.IsExistModificationNumber(projectModificationModel.ProjectGuid, projectModificationModel.ModificationNumber);

                if (isExistModificationNumber)
                {
                    throw new ArgumentException("", " Found Duplicate Modification Number !!");
                }

                if (ModelState.IsValid)
                {
                    Guid id = Guid.NewGuid();
                    projectModificationModel.ProjectModificationGuid = id;
                    projectModificationModel.CreatedOn = DateTime.Now;
                    projectModificationModel.CreatedBy = id;
                    projectModificationModel.UpdatedOn = DateTime.Now;
                    projectModificationModel.UpdatedBy = id;
                    projectModificationModel.IsActive = true;
                    projectModificationModel.IsDeleted = false;
                    projectModificationModel.AwardAmount = projectModificationModel.AwardAmount ?? 0;
                    if (projectModificationModel.FileToUpload != null && projectModificationModel.FileToUpload.Count > 0)
                    {
                        foreach (var formFile in projectModificationModel.FileToUpload)
                        {
                            var uploadPath = string.Format(
                                $@"DocumentRoot\{projectModificationModel.ContractNumber}\{projectModificationModel.ProjectNumber}\ProjectModification\{projectModificationModel.ModificationTitle}-{projectModificationModel.ModificationNumber}");
                            var relativeFilePath =
                                _fileService.FilePost(uploadPath, formFile);
                            filePath.Add(relativeFilePath);
                        }
                        projectModificationModel.UploadedFileName = string.Join(",", filePath);
                    }
                    projectModificationModel.UploadedFileName = projectModificationModel.UploadedFileName; // file path..

                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }

                    var projectModificationEntity = _mapper.Map<ProjectModificationModel>(projectModificationModel);
                    _projectModificationService.Add(projectModificationEntity);

                    return Ok(new { status = ResponseStatus.success.ToString(), message = "Successfully Added !!" });
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return BadRequest(_configuration.GetSection("ExceptionErrorMessage").Value);
            }
        }


        [HttpGet]
        public ActionResult Edit(Guid id)
        {
            ProjectModificationViewModel projectModificationModel = new ProjectModificationViewModel();
            if (id != Guid.Empty)
            {
                var projectModificationEntity = _projectModificationService.GetDetailById(id);
                projectModificationModel = _mapper.Map<ProjectModificationViewModel>(projectModificationEntity);
                projectModificationModel.ContractGuid = projectModificationModel.ProjectGuid;

                projectModificationModel.ContractNumber =
                    _contractService.GetDetailsForProjectByContractId(projectModificationModel.ContractGuid ?? Guid.Empty).ContractNumber;

                projectModificationModel.ProjectNumber =
                   _contractService.GetDetailById(projectModificationModel.ProjectGuid).BasicContractInfo.ProjectNumber;
            }
            try
            {
                return PartialView(projectModificationModel);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return View(projectModificationModel);
            }
        }


        [HttpPost]
        public IActionResult Edit([FromForm]ProjectModificationViewModel projectModificationModel)
        {
            try
            {
                List<string> filePath = new List<string>();

                //check for duplicate modification number..
                var projectModification =
                    _projectModificationService.GetDetailById(projectModificationModel.ProjectModificationGuid);
                if (projectModification != null)
                {
                    if (!projectModification.ModificationNumber.Equals(projectModificationModel.ModificationNumber))
                    {
                        if (_projectModificationService.IsExistModificationNumber(projectModificationModel.ProjectGuid,
                            projectModificationModel.ModificationNumber))
                        {
                            throw new ArgumentException("", " Found Duplicate Modification Number !!");
                        }
                    }
                }

                if (ModelState.IsValid)
                {
                    Guid id = Guid.NewGuid();
                    projectModificationModel.UpdatedOn = DateTime.Now;
                    projectModificationModel.UpdatedBy = id;

                    if (projectModificationModel.FileToUpload != null && projectModificationModel.FileToUpload.Count > 0)
                    {
                        foreach (var formFile in projectModificationModel.FileToUpload)
                        {
                            var uploadPath = string.Format(
                                $@"DocumentRoot\{projectModificationModel.ContractNumber}\{projectModificationModel.ProjectNumber}\ProjectModification\{projectModificationModel.ModificationTitle}-{projectModificationModel.ModificationNumber}");
                            var relativeFilePath =
                                _fileService.FilePost(uploadPath, formFile);
                            filePath.Add(relativeFilePath);
                        }
                        projectModificationModel.UploadedFileName = string.Join(",", filePath);
                    }
                    projectModificationModel.UploadedFileName = projectModificationModel.UploadedFileName; // file path..

                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }

                    var projectModificationEntity = _mapper.Map<ProjectModificationModel>(projectModificationModel);
                    _projectModificationService.Edit(projectModificationEntity);

                    return Ok(new { status = ResponseStatus.success.ToString(), message = "Successfully Updated !!" });
                }
                else
                {
                    return View(projectModificationModel);
                }
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                ModelState.Clear();
                ModelState.AddModelError("", ex.Message);
                return BadRequestFormatter.BadRequest(this, ModelState);
            }
        }
        [HttpGet]
        public ActionResult Details(Guid id)
        {
            ProjectModificationViewModel projectModificationModel = new ProjectModificationViewModel();
            try
            {
                var projectModificationEntity = _projectModificationService.GetDetailById(id);
                projectModificationModel = _mapper.Map<ProjectModificationViewModel>(projectModificationEntity);
                return PartialView(projectModificationModel);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return View(projectModificationModel);
            }
        }
        [HttpPost]
        public IActionResult Delete([FromBody] Guid[] ids)
        {
            try
            {
                _projectModificationService.Delete(ids);
                return Ok(new { status = ResponseStatus.success.ToString(), message = "Successfully Deleted !!" });
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return BadRequest(_configuration.GetSection("ExceptionErrorMessage").Value);
            }
        }
        [HttpPost]
        public IActionResult Disable([FromBody] Guid[] ids)
        {
            try
            {
                _projectModificationService.Disable(ids);
                return Ok(new { status = ResponseStatus.success.ToString(), message = "Successfully Disabled !!" });
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return BadRequest(_configuration.GetSection("ExceptionErrorMessage").Value);
            }
        }
        [HttpPost]
        public IActionResult Enable([FromBody] Guid[] ids)
        {
            try
            {
                _projectModificationService.Enable(ids);
                return Ok(new { status = ResponseStatus.success.ToString(), message = "Successfully Enabled !!" });
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return BadRequest(_configuration.GetSection("ExceptionErrorMessage").Value);
            }
        }

        #endregion

        [HttpGet]
        public IActionResult DownloadDocument(string filePath, string fileName)
        {
            try
            {
                return _fileService.GetFile(fileName, filePath);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
