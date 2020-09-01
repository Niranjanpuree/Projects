using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using Northwind.Core.Utilities;
using Northwind.Web.Infrastructure.Helpers;
using Northwind.Web.Infrastructure.Models;
using Northwind.Web.Infrastructure.Models.ViewModels;
using Northwind.Web.Models.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Northwind.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class InboxController : Controller
    {
        private readonly INotificationBatchService _notificationBatchService;
        private readonly INotificationMessageService _notificationMessageService;
        private readonly IDistributionListService _distributionListService;
        private readonly INotificationTemplatesService _notificationTemplatesService;
        private readonly INotificationService _notificationService;
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;


        public InboxController(
            INotificationBatchService notificationBatchService,
            INotificationMessageService notificationMessageService,
            IDistributionListService distributionListService,
            INotificationTemplatesService notificationTemplatesService,
            INotificationService notificationService,
            IUserService userService,
            IConfiguration configuration
        )
        {
            _notificationBatchService = notificationBatchService;
            _notificationMessageService = notificationMessageService;
            _distributionListService = distributionListService;
            _notificationTemplatesService = notificationTemplatesService;
            _notificationService = notificationService;
            _userService = userService;
            _configuration = configuration;
        }
        // GET: /<controller>/
        public IActionResult Index(Guid notificationMessageGuid, string searchValue)
        {
            try
            {
                //if id is null then user directly visited to the message box page..
                if (notificationMessageGuid == Guid.Empty)
                {
                    var loggedUser = UserHelper.CurrentUserGuid(HttpContext);
                    //                    var loggedUser = new Guid("73526DCB-3D7E-48C0-B4A0-4BE20AE3F85C");
                    var notificationMessage = _notificationMessageService.GetLatestDesktopNotification(loggedUser);
                    if (notificationMessage != null)
                        notificationMessageGuid = notificationMessage.NotificationMessageGuid;
                }

                ViewBag.NotificationMessageGuid = notificationMessageGuid;
                return View();
            }
            catch (Exception ex)
            {
                return BadRequestFormatter.BadRequest(this,ex);
            }
        }
        public IActionResult GetMessageByNotificationMessageId(Guid notificationMessageGuid)
        {
            try
            {
                var notificationMessageById =
                    _notificationMessageService.GetByNotificationMessageId(notificationMessageGuid);

                if (notificationMessageById != null)
                {
                    //this block is required to get the actual User name of user who created the notifications
                    var notificationMessage =
                        Models.ObjectMapper<NotificationMessage, NotificationMessageViewModel>.Map(
                            notificationMessageById);
                    var user = _userService.GetUserByUserGuid(notificationMessage.CreatedBy);

                    notificationMessage.CreatedByName =
                        Infrastructure.Helpers.FormatHelper.FormatFullName(user.Firstname, string.Empty, user.Lastname);
                    notificationMessage.CreatedOnFormatDateTime = Infrastructure.Helpers.FormatHelper.FormatDateTime(
                        notificationMessage.CreatedOn.ToString("MM/dd/yyyy"),
                        notificationMessage.CreatedOn.ToString("HH:m:s tt zzz"));

                    return Json(notificationMessage);
                }

                return Json(new NotificationMessage());
            }
            catch (Exception ex)
            {
                return BadRequestFormatter.BadRequest(this, ex);
            }
        }
    }
}
