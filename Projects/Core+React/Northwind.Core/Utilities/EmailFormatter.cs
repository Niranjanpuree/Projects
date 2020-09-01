using Northwind.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Utilities
{
    public class EmailFormatter
    {
        public static string GetContentForNotify(string template, NotificationTemplatesDetail model)
        {
            StringBuilder content = new StringBuilder(template);
           
            var receiverDisplayName = (string.IsNullOrEmpty(model.ReceiverDisplayName) ? string.Empty : model.ReceiverDisplayName);

            content.Replace("{RECEIVER_NAME}", (string.IsNullOrEmpty(model.ReceiverDisplayName) ? string.Empty : model.ReceiverDisplayName));
            content.Replace("{SUBMITTED_NAME}", (string.IsNullOrEmpty(model.SubmittedByName) ? string.Empty : model.SubmittedByName));
            content.Replace("{CONTRACT_NUMBER}", (string.IsNullOrEmpty(model.ContractNumber) ? string.Empty : model.ContractNumber));
            content.Replace("{AWARDING_AGENCY}", (string.IsNullOrEmpty(model.AwardingAgency) ? string.Empty : model.AwardingAgency));
            content.Replace("{FUNDING_AGENCY}", (string.IsNullOrEmpty(model.FundingAgency) ? string.Empty : model.FundingAgency));
            content.Replace("{TASK_ORDERNUMBER}", (string.IsNullOrEmpty(model.TaskOrderNumber) ? string.Empty : model.TaskOrderNumber));
            content.Replace("{PROJECT_NUMBER}", (string.IsNullOrEmpty(model.ProjectNumber) ? string.Empty : model.ProjectNumber));
            content.Replace("{CONTRACT_TITLE}", (string.IsNullOrEmpty(model.ContractTitle) ? string.Empty : model.ContractTitle));
            content.Replace("{TASKORDER_TITLE}", (string.IsNullOrEmpty(model.ContractTitle) ? string.Empty : model.ContractTitle));
            content.Replace("{TITLE}", (string.IsNullOrEmpty(model.ContractTitle) ? string.Empty : model.ContractTitle));
            content.Replace("{SUBMITTED_BY}", (string.IsNullOrEmpty(model.SubmittedByName) ? string.Empty : model.SubmittedByName));
            content.Replace("{STATUS}", (string.IsNullOrEmpty(model.Status) ? string.Empty : model.Status));
            content.Replace("{KEY_PERSONNELLIST}", (string.IsNullOrEmpty(model.AdditionalUser) ? string.Empty : model.AdditionalUser));
            content.Replace("{JOBREQUEST_URL}", (string.IsNullOrEmpty(model.RedirectUrlPath) ? string.Empty : model.RedirectUrlPath));
            content.Replace("{NOTIFY_TO}", (string.IsNullOrEmpty(model.ReceiverDisplayName) ? string.Empty : model.ReceiverDisplayName));
            content.Replace("{NOTIFY_OTHER}", (string.IsNullOrEmpty(model.AdditionalUser) ? string.Empty : model.AdditionalUser));
            content.Replace("{ADDITIONAL_RECIPIENT}", (string.IsNullOrEmpty(model.AdditionalUser) ? string.Empty : model.AdditionalUser));
            content.Replace("{ADDITIONAL_MESSAGE}", (string.IsNullOrEmpty(model.AdditionalMessage) ? string.Empty : model.AdditionalMessage));
            content.Replace("{REQUESTEDBY_NAME}", (string.IsNullOrEmpty(model.ReceiverDisplayName) ? string.Empty : model.ReceiverDisplayName));
            content.Replace("{CONTRACT_TYPE}", (string.IsNullOrEmpty(model.ContractType) ? string.Empty : model.ContractType));
            content.Replace("{Contract_type}", (string.IsNullOrEmpty(model.ContractType) ? string.Empty : model.ContractType));
            content.Replace("{PROJECT_NUMBER}", (string.IsNullOrEmpty(model.ProjectNumber) ? string.Empty : model.ProjectNumber));
            content.Replace("{ThresholdAward_Amount}", (string.IsNullOrEmpty(model.ThresholdAmount.ToString()) ? string.Empty : model.ThresholdAmount.ToString()));
            content.Replace("{MOD_NUMBER}", (string.IsNullOrEmpty(model.ModNumber) ? string.Empty : model.ModNumber));
            content.Replace("{Redirect_Url}", (string.IsNullOrEmpty(model.RedirectUrlPath) ? string.Empty : model.RedirectUrlPath));
            content.Replace("{REDIRECT_URL}", (string.IsNullOrEmpty(model.RedirectUrlPath) ? string.Empty : model.RedirectUrlPath));
            content.Replace("{CONTRACT_DESCRIPTION}", (string.IsNullOrEmpty(model.Description) ? string.Empty : model.Description));
            return content.ToString();
        }

    }
}
