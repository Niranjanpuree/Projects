using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using Northwind.PFS.Web.Models.ViewModels;
using Northwind.PFS.Web.Models;
using Northwind.Core.Interfaces.CrossSiteInterface;
using Northwind.CostPoint.Interfaces;
using Northwind.Web.Infrastructure.Models;
using Northwind.Core.Models;

namespace Northwind.PFS.Web.Areas.CostPoint.Controllers
{
    public class ProjectController : Controller
    {
        IProjectServiceCP _projectService;
        IMapper _mapper;
        private readonly IRecentActivityService _recentActivityService;
        private readonly IContractServiceCrossSite _contractsService;
        public ProjectController(IProjectServiceCP projectService, IMapper mapper, IRecentActivityService recentActivityService, IContractServiceCrossSite contractsService)
        {
            _projectService = projectService;
            _mapper = mapper;
            _recentActivityService = recentActivityService;
            _contractsService = contractsService;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        [HttpGet]
        public IActionResult Get(string searchValue, int pageSize, int skip, int take, string orderBy, string dir, string additionalFilter)
        {
            try
            {
                if (orderBy == "")
                {
                    orderBy = "ProjectNumber";
                }
                var userGuid = GetUserGuid();
                var result = _contractsService.GetProjects(searchValue, pageSize, skip, take, orderBy, dir, new List<AdvancedSearchRequest>(), GetUserGuid(), additionalFilter, true);
                var projectList = new List<ProjectViewModel>();
                foreach(var proj in result)
                {
                    var p = new ProjectViewModel
                    {
                        Company = proj.Organisation.Title,
                        ContractAmount = proj.AwardAmount.HasValue? proj.AwardAmount.Value: 0,
                        ContractGuid = proj.ContractGuid,
                        ContractNumber = proj.ContractNumber,
                        Description = proj.Description,
                        FundedAmount = proj.FundingAmount.HasValue? proj.FundingAmount.Value:0,
                        IsFavorite = proj.IsFavorite,
                        OrgId = proj.Organisation.Name,
                        POPEndDate = proj.POPEnd,
                        POPStartDate = proj.POPStart,
                        ProjectControl = proj.ProjectControls.DisplayName,
                        ProjectManager = proj.ProjectManager.DisplayName,
                        ProjectName = proj.ContractTitle,
                        ProjectNumber = proj.ProjectNumber,
                        RegionalManager = proj.RegionalManager.DisplayName
                    };
                    projectList.Add(p);
                }

                //To Do
                var count = _contractsService.GetProjectCount(searchValue, new List<AdvancedSearchRequest>(), GetUserGuid(), additionalFilter, true);
                //var count = 3;
                return new JsonResult(new { result = projectList, count });
            }
            catch (Exception ex)
            {
                ModelState.Clear();
                ModelState.AddModelError("", ex.Message);
                return BadRequestFormatter.BadRequest(this, ex);
            }
        }

        [Authorize]
        [HttpPost]
        public IActionResult Get(string searchValue, int pageSize, int skip, int take, string orderBy, string dir, [FromBody] List<AdvancedSearchRequest> postValue, string additionalFilter)
        {
            try
            {
                if (orderBy == "")
                {
                    orderBy = "ProjectNumber";
                }
                var userGuid = GetUserGuid();
                var result = _contractsService.GetProjects(searchValue, pageSize, skip, take, orderBy, dir, postValue, GetUserGuid(), additionalFilter, true);
                var projectList = new List<ProjectViewModel>();
                foreach (var proj in result)
                {
                    var p = new ProjectViewModel
                    {
                        Company = proj.Organisation.Title,
                        ContractAmount = proj.AwardAmount.HasValue ? proj.AwardAmount.Value : 0,
                        ContractGuid = proj.ContractGuid,
                        ContractNumber = proj.ContractNumber,
                        Description = proj.Description,
                        FundedAmount = proj.FundingAmount.HasValue ? proj.FundingAmount.Value : 0,
                        IsFavorite = proj.IsFavorite,
                        OrgId = proj.Organisation.Name,
                        POPEndDate = proj.POPEnd,
                        POPStartDate = proj.POPStart,
                        ProjectControl = proj.ProjectControls.DisplayName,
                        ProjectManager = proj.ProjectManager.DisplayName,
                        ProjectName = proj.ContractTitle,
                        ProjectNumber = proj.ProjectNumber,
                        RegionalManager = proj.RegionalManager.DisplayName
                    };
                    projectList.Add(p);
                }
                var count = _contractsService.GetProjectCount(searchValue, postValue, GetUserGuid(), additionalFilter, true);

                return new JsonResult(new { result = projectList, count });
            }
            catch (Exception ex)
            {
                ModelState.Clear();
                ModelState.AddModelError("", ex.Message);
                return BadRequestFormatter.BadRequest(this, ex);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">Project Number</param>
        /// <returns></returns>
        public IActionResult Details(string id)
        {
            ViewBag.ProjectNumber = id;
            var project = _contractsService.GetProject(id);
            if(project == null)
            {
                return Redirect("~/project");
            }
            var p = new ProjectViewModel
            {
                Company = project.Organisation.Title,
                ContractAmount = project.AwardAmount.HasValue ? project.AwardAmount.Value : 0,
                ContractGuid = project.ContractGuid,
                ContractNumber = project.ContractNumber,
                Description = project.Description,
                FundedAmount = project.FundingAmount.HasValue ? project.FundingAmount.Value : 0,
                IsFavorite = project.IsFavorite,
                OrgId = project.Organisation.Name,
                POPEndDate = project.POPEnd,
                POPStartDate = project.POPStart,
                ProjectControl = project.ProjectControls == null ? "" : project.ProjectControls.DisplayName,
                ProjectManager = project.ProjectManager == null ? "" : project.ProjectManager.DisplayName,
                ProjectName = project.ContractTitle,
                ProjectNumber = project.ProjectNumber,
                RegionalManager = project.RegionalManager == null ? "" : project.RegionalManager.DisplayName
            };
            return View(p);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Favorite([FromBody]ProjectViewModel projectViewModel)
        {
            try
            {
                var contractGuid = _contractsService.GetContractGuidByProjectNumber(projectViewModel.ProjectNumber);
                if (contractGuid != Guid.Empty) {
                    var userGuid = GetUserGuid();
                    var activity = new RecentActivity
                    {
                        CreatedBy = userGuid,
                        CreatedOn = DateTime.Now,
                        Entity = "PFS-Project",
                        EntityGuid = contractGuid,
                        IsDeleted = false,
                        UpdatedBy = userGuid,
                        UpdatedOn = DateTime.Now,
                        UserAction = EnumGlobal.ActivityType.MyFavorite.ToString(),
                        UserGuid = userGuid
                    };
                    _recentActivityService.InsertRecentActivity(activity);

                    _projectService.UpdateContractGuidByProjectNumber(projectViewModel.ProjectNumber,contractGuid);
                    return new JsonResult(new { status = true });
                }
                return new JsonResult(new { status = false });
            }
                
            catch (Exception ex)
            {
                ModelState.Clear();
                ModelState.AddModelError("", ex.Message);
                return BadRequestFormatter.BadRequest(this, ex);
            }
        }

        [Authorize]
        [HttpPost]
        public IActionResult Unfavorite([FromBody]ProjectViewModel projectViewModel)
        {
            try
            {
                var contractGuid = _contractsService.GetContractGuidByProjectNumber(projectViewModel.ProjectNumber);
                var userGuid = GetUserGuid();
                var activity = _recentActivityService.GetRecentActivityByEntityGuid(contractGuid,userGuid, "PFS-Project", EnumGlobal.ActivityType.MyFavorite.ToString());
                if (activity != null)
                {
                    activity.IsDeleted = true;
                    activity.UpdatedBy = userGuid;
                    activity.UpdatedOn = DateTime.Now;
                    _recentActivityService.UpdateRecentActivity(activity);
                }
                return new JsonResult(new { status = true });
            }
            catch (Exception ex)
            {
                ModelState.Clear();
                ModelState.AddModelError("", ex.Message);
                return BadRequestFormatter.BadRequest(this, ex);
            }
        }


        private Guid GetUserGuid()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Guid.Parse(User.Identity.Name);
            }
            return Guid.Empty;
        }
    }
}