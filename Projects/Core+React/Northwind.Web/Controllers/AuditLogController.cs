using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Northwind.Core.AuditLog.Interfaces;
using Northwind.Core.Models;
using Northwind.Web.Helpers;
using Northwind.Web.Infrastructure.Authorization;
using Northwind.Web.Infrastructure.Helpers;
using Northwind.Web.Infrastructure.Models;
using Northwind.Web.Models;
using Northwind.Web.Models.ViewModels.AuditLog;

namespace Northwind.Web.Controllers
{
    [Authorize]
    public class AuditLogController : Controller
    {
        private readonly IAuditLogService _auditLogService;
        public AuditLogController(IAuditLogService auditLogService)
        {
            _auditLogService = auditLogService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Get(string searchValue, int pageSize, int skip, int take, string sortField, string dir)
        {
            try
            {
                var loggedUser = UserHelper.CurrentUserGuid(HttpContext);
                var result = _auditLogService.GetAll(searchValue, pageSize, skip, take, sortField, dir);

                var lstAuditLog = result.Select(x => new AuditLog
                {
                    AuditLogGuid = x.AuditLogGuid,
                    TimeStamp = x.TimeStamp,
                    Resource = x.Resource,
                    ResourceId = x.ResourceId,
                    Actor = x.Actor,
                    ActorId = x.ActorId,
                    IpAddress = x.IpAddress,
                    Action = x.Action,
                    ActionId = x.ActionId,
                    ActionResult = x.ActionResult,
                    ActionResultReason = x.ActionResultReason,
                    //AdditionalInformation = string.Format("<a href=\"{0}\">{1}</a>", x.AdditionalInformationURl, x.AdditionalInformation),
                    AdditionalInformation = string.IsNullOrEmpty(x.AdditionalInformationURl) ? "" : string.Format("<a href=\"{0}\">{1}</a>", x.AdditionalInformationURl, "Click to view affected resource.."),
                    AdditionalInformationURl = x.AdditionalInformationURl
                }).ToList();

                return Ok(new { result = lstAuditLog, count = _auditLogService.TotalRecord(searchValue) });
            }
            catch (Exception ex)
            {
                ModelState.Clear();
                ModelState.AddModelError("", ex.Message);
                return BadRequestFormatter.BadRequest(this, ex);
            }
        }
        //[Secure("auditLog", "list")]
        [HttpPost]
        public IActionResult Get(string searchValue, int pageSize, int skip, int take, string sortField, string dir, [FromBody] List<AdvancedSearchRequest> postValue, string additionalFilter = "")
        {
            try
            {
                var loggedUserGuid = UserHelper.CurrentUserGuid(HttpContext);
                var auditLogList = _auditLogService.GetAll(searchValue, pageSize, skip, take, sortField, dir, postValue, additionalFilter);
                var count = _auditLogService.GetAdvanceSearchCount(searchValue, postValue, Guid.Empty, additionalFilter);

                return Ok(new { result = auditLogList, count = count });
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