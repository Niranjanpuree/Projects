using Northwind.Core.Entities;
using Northwind.Core.Entities.ContractRefactor;
using Northwind.Core.Interfaces;
using Northwind.Core.Interfaces.ContractRefactor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Northwind.Core.Entities.GenericNotification;
using static Northwind.Core.Utilities.EmailFormatter;

namespace Northwind.Core.Services
{
    public class GenericNotificationService : IGenericNotificationService
    {
        private readonly INotificationTemplatesService _notificationTemplatesService;
        private readonly INotificationBatchService _notificationBatchService;
        private readonly INotificationMessageService _notificationMessageService;
        private readonly IContractsService _contractsService;
        private readonly IUserService _userService;
        private readonly IEmailSender _emailSender;

        public GenericNotificationService(INotificationTemplatesService notificationTemplatesService,
            INotificationMessageService notificationMessageService, INotificationBatchService notificationBatchService,
            IContractsService contractsService, IUserService userService, IEmailSender emailSender)
        {
            _notificationTemplatesService = notificationTemplatesService;
            _notificationMessageService = notificationMessageService;
            _notificationBatchService = notificationBatchService;
            _contractsService = contractsService;
            _emailSender = emailSender;
            _userService = userService;
        }


        private Guid AddNotificationBatch(GenericNotificationViewModel model)
        {
            //Add the  Notification Batch .....
            var notificationTemplate = _notificationTemplatesService.GetByKey(model.NotificationTemplateKey);
            if (notificationTemplate != null)
            {
                NotificationBatch notificationBatch = new NotificationBatch();
                Guid batchGuid = Guid.NewGuid();
                notificationBatch.StartDate = model.CurrentDate;
                if (!string.IsNullOrEmpty(model.NotificationTemplateKey))
                {
                    notificationBatch.ResourceType = model.NotificationTemplateKey.Split('.')[0];
                    notificationBatch.ResourceAction = model.NotificationTemplateKey.Split('.')[1];
                }
                notificationBatch.AdditionalMessage = "";
                notificationBatch.ResourceId = model.ResourceId;
                notificationBatch.NotificationBatchGuid = batchGuid;
                notificationBatch.NotificationTemplateGuid = notificationTemplate.NotificationTemplateGuid;
                notificationBatch.CreatedBy = model.CurrentUserGuid;
                notificationBatch.CreatedOn = model.CurrentDate;

                _notificationBatchService.Add(notificationBatch);
                return batchGuid;
            }
            return Guid.Empty;
        }

        public bool AddNotificationMessage(GenericNotificationViewModel model)
        {
            foreach (var item in model.IndividualRecipients)
            {
                var senderDetails = _userService.GetUserByUserGuid(model.CurrentUserGuid);
                var link = model.RedirectUrl;
                Guid notificationBatchGuid = AddNotificationBatch(model);

                if (notificationBatchGuid != Guid.Empty)
                {
                    // Notification Templates
                    var notificationTemplate = _notificationTemplatesService.GetNotificationTemplateByKey(model.NotificationTemplateKey);
                    model.NotificationTemplatesDetail.ReceiverDisplayName = item.DisplayName;
                    model.NotificationTemplatesDetail.SubmittedByName = senderDetails.DisplayName;
                    model.NotificationTemplatesDetail.RedirectUrlPath = model.RedirectUrl;

                    var message = GetContentForNotify(notificationTemplate.Message, model.NotificationTemplatesDetail);
                    var subject = GetContentForNotify(notificationTemplate.Subject, model.NotificationTemplatesDetail);

                    var notificationMessage = new NotificationMessage
                    {
                        NotificationBatchGuid = notificationBatchGuid,
                        UserGuid = item.UserGuid,
                        Subject = subject,
                        Message = message,
                        AdditionalMessage = "",
                        Status = false,
                        UserResponse = false,
                        NextAction = model.CurrentDate,
                        CreatedOn = model.CurrentDate,
                        CreatedBy = model.CurrentUserGuid,
                    };
                    if (model.SendEmail)
                    {
                         _emailSender.SendEmailAsync(item.WorkEmail, item.DisplayName, subject, message);
                    }
                    return _notificationMessageService.Add(notificationMessage);
                }
            }
            return false;
        }

    }
}
