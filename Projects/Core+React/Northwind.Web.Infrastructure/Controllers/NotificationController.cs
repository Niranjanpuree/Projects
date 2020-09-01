using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Northwind.Core.Interfaces;
using Northwind.Core.Utilities;
using Northwind.Web.Infrastructure.Helpers;
using Northwind.Web.Infrastructure.Models.ViewModels;

namespace Northwind.Web.Infrastructure
{
    [Authorize]
    public class NotificationController : Controller
    {
        private readonly INotificationMessageService _notificationMessageService;
        private readonly IUserService _userService; 
        public NotificationController(INotificationMessageService notificationMessageService, IUserService userService)
        {
            _notificationMessageService = notificationMessageService;
            _userService = userService;
        }

        //get unread message for desktop bell icon ..
        public IActionResult GetUnReadDesktopNotification()
        {
            var loggedUser = UserHelper.CurrentUserGuid(HttpContext);

            //this block is required to get the actual User name of user who created the notifications
            var notificationMessages = _notificationMessageService.GetUnReadDesktopNotification(loggedUser);
            List<NotificationMessageViewModel> notificationMessageViewModels = new List<NotificationMessageViewModel>();
            foreach (var ob in notificationMessages)
            {
                var notificationMessage = new NotificationMessageViewModel
                {
                    AdditionalMessage=ob.AdditionalMessage,
                    CreatedBy=ob.CreatedBy,
                    CreatedOn=ob.CreatedOn,
                    DistributionListGuid=ob.DistributionListGuid,
                    Message=ob.Message,
                    NextAction=ob.NextAction,
                    NotificationBatchGuid=ob.NotificationBatchGuid,
                    NotificationMessageGuid=ob.NotificationMessageGuid,
                    Status=ob.Status,
                    Subject=ob.Subject,
                    UserGuid=ob.UserGuid,
                    UserResponse=ob.UserResponse
                };
                var user = _userService.GetUserByUserGuid(notificationMessage.CreatedBy);

                notificationMessage.CreatedByName =
                    Core.Utilities.FormatHelper.FormatFullName(user.Firstname, string.Empty, user.Lastname);
                notificationMessage.CreatedOnFormatDateTime = Core.Utilities.FormatHelper.FormatDateTime(
                    notificationMessage.CreatedOn.ToString("MM/dd/yyyy"),
                    notificationMessage.CreatedOn.ToString("HH:m:s tt zzz"));
                notificationMessageViewModels.Add(notificationMessage);
            }

            return Json(notificationMessageViewModels);
        }
    }
}