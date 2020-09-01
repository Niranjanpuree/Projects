using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Northwind.Core.AuditLog.Interfaces;
using Northwind.Core.Interfaces;
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
    public class EventLogController : Controller
    {
        private readonly IEventLogService _eventLogService;
        private readonly IUserService _userService;
        public EventLogController(IEventLogService eventLogService,IUserService userService)
        {
            _eventLogService = eventLogService;
            _userService = userService;
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
                var result = _eventLogService.GetAll(searchValue, pageSize, skip, take, sortField, dir);

                var lstAuditLog = result.Select(x => new EventLog
                {
                    EventGuid = x.EventGuid,
                    EventDate = x.EventDate,
                    Resource = x.Resource,
                    Application = x.Application,
                    Message = x.Message,
                    UserName = _userService.GetUserByUserGuid(x.UserGuid).DisplayName,
                    StackTrace = x.StackTrace,
                    Action = x.Action
                    
                }).ToList();

                return Ok(new { result = lstAuditLog, count = _eventLogService.TotalRecord(searchValue) });
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
                var auditLogList = _eventLogService.GetAll(searchValue, pageSize, skip, take, sortField, dir, postValue, additionalFilter);
                var count = _eventLogService.GetAdvanceSearchCount(searchValue, postValue, Guid.Empty, additionalFilter);

                return Ok(new { result = auditLogList, count = count });
            }
            catch (Exception ex)
            {
                ModelState.Clear();
                ModelState.AddModelError("", ex.Message);
                return BadRequestFormatter.BadRequest(this, ex);
            }
        }

        public IActionResult Details(Guid EventGuid)
        {
          var eventLogDetails =  _eventLogService.GetDetails(EventGuid);
            return PartialView(eventLogDetails);

        }

    }
}