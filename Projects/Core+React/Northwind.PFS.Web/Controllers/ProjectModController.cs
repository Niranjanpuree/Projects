using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Northwind.PFS.Web.Models.ViewModels;
using Northwind.PFS.Web.Models;
using Northwind.CostPoint.Interfaces;
using Northwind.Web.Infrastructure.Models;
using Northwind.Core.Interfaces.CrossSiteInterface;

namespace Northwind.PFS.Web.Areas.PFS.Controllers
{
    public class ProjectModController : Controller
    {
        private readonly IProjectModServiceCP _projectModService;
        private readonly IContractServiceCrossSite _contractsService;
        IMapper _mapper;
        public ProjectModController(IProjectModServiceCP projectModService, IContractServiceCrossSite contractsService, IMapper mapper)
        {
            _projectModService = projectModService;
            _contractsService = contractsService;
            _mapper = mapper;
        }

        public IActionResult Get(string projectNumber, string searchValue, int skip, int take, string orderBy, string dir)
        {
            try
            {
                int rowIndex = 0;
                if (string.IsNullOrEmpty(orderBy))
                {
                    orderBy = "PROJ_MOD_ID";
                }
                var project = _contractsService.GetProject(projectNumber);
                if(project == null)
                {
                    throw new Exception("Unable to find project");
                }
                var list = new List<ProjectModViewModel>();
                list.Add(new ProjectModViewModel
                {
                    AwardAmount = project.AwardAmount.HasValue ? project.AwardAmount.Value : 0,
                    Description = project.Description,
                    AwardDate = project.POPStart.HasValue ? project.POPStart.Value : DateTime.MinValue,
                    FundedAmount = project.FundingAmount.HasValue ? project.FundingAmount.Value : 0,
                    ModNumber = project.ProjectNumber,
                    ProjectNumber = project.ProjectNumber,
                    POPEndDate = project.POPEnd.HasValue ? project.POPEnd.Value : DateTime.MinValue,
                    POPStartDate = project.POPStart.HasValue ? project.POPStart.Value : DateTime.MinValue,
                    ProjectModId = "",
                    Title = "Base",
                    Currency = project.Currency,
                    Id = rowIndex,
                    Attachments = new List<ProjectModAttachments>() { new ProjectModAttachments { Title = "File1", DownloadLink = "http://google.com/download" } }
                });
                var result = _projectModService.GetProjectMods(projectNumber, searchValue, skip, take, orderBy, dir, new List<Core.Models.AdvancedSearchRequest>());
                foreach(var pm in result)
                {
                    rowIndex++;
                    var item = new ProjectModViewModel
                    {
                        AwardAmount = pm.AwardAmount,
                        Description = pm.Description,
                        AwardDate = pm.POPStartDate,
                        FundedAmount = pm.FundedAmount,
                        ModNumber = pm.ModNumber,
                        ProjectNumber = project.ProjectNumber,
                        POPEndDate = pm.POPEndDate,
                        POPStartDate = pm.POPStartDate,
                        ProjectModId = pm.ProjectModId,
                        Title = pm.Title,
                        Currency = "USD",
                        Id = rowIndex,
                        Attachments = new List<ProjectModAttachments>() { new ProjectModAttachments {  Title="File1", DownloadLink="http://google.com/download" } }
                    };
                    list.Add(item);
                }
                var count = _projectModService.GetProjectModsCount(projectNumber, searchValue, new List<Core.Models.AdvancedSearchRequest>());
                return Json(new { result = list, count = count + 1 });
            }
            catch (Exception ex)
            {
                ModelState.Clear();
                ModelState.AddModelError("", ex.Message);
                return BadRequestFormatter.BadRequest(this, ex);
            }
        }

        [HttpPost]
        public IActionResult Get(string projectNumber, string searchValue, int skip, int take, string orderBy, string dir,[FromBody] List<Core.Models.AdvancedSearchRequest> postValue)
        {
            try
            {
                int rowIndex = 0;
                if (string.IsNullOrEmpty(orderBy))
                {
                    orderBy = "ModNumber";
                }
                var project = _contractsService.GetProject(projectNumber);
                if (project == null)
                {
                    throw new Exception("Unable to find project");
                }
                var list = new List<ProjectModViewModel>();
                list.Add(new ProjectModViewModel
                {
                    AwardAmount = project.AwardAmount.HasValue ? project.AwardAmount.Value : 0,
                    Description = project.Description,
                    AwardDate = project.POPStart.HasValue ? project.POPStart.Value : DateTime.MinValue,
                    FundedAmount = project.FundingAmount.HasValue ? project.FundingAmount.Value : 0,
                    ModNumber = project.ProjectNumber,
                    ProjectNumber = project.ProjectNumber,
                    POPEndDate = project.POPEnd.HasValue ? project.POPEnd.Value : DateTime.MinValue,
                    POPStartDate = project.POPStart.HasValue ? project.POPStart.Value : DateTime.MinValue,
                    ProjectModId = "",
                    Title = "Base",
                    Currency = project.Currency,
                    Id = rowIndex,
                    Attachments = new List<ProjectModAttachments>() { new ProjectModAttachments { Title = "File1", DownloadLink = "http://google.com/download" } }
                });
                var result = _projectModService.GetProjectMods(projectNumber, searchValue, skip, take, orderBy, dir, postValue);
                foreach (var pm in result)
                {
                    rowIndex++;
                    var item = new ProjectModViewModel
                    {
                        AwardAmount = pm.AwardAmount,
                        Description = pm.Description,
                        AwardDate = pm.POPStartDate,
                        FundedAmount = pm.FundedAmount,
                        ModNumber = pm.ModNumber,
                        ProjectNumber = project.ProjectNumber,
                        POPEndDate = pm.POPEndDate,
                        POPStartDate = pm.POPStartDate,
                        ProjectModId = pm.ProjectModId,
                        Title = pm.Title,
                        Currency = "USD",
                        Id = rowIndex,
                        Attachments = new List<ProjectModAttachments>() { new ProjectModAttachments { Title = "File1", DownloadLink = "http://google.com/download" } }
                    };
                    list.Add(item);
                }
                var count = _projectModService.GetProjectModsCount(projectNumber, searchValue, postValue);
                return Json(new { result = list, count = count + 1 });
            }
            catch (Exception ex)
            {
                ModelState.Clear();
                ModelState.AddModelError("", ex.Message);
                return BadRequestFormatter.BadRequest(this, ex);
            }
        }
    }
}