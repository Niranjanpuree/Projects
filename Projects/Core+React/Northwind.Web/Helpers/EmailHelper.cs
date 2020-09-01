using Northwind.Core.Entities.ContractRefactor;
using Northwind.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind.Web.Helpers
{
    public class EmailHelper
    {
        public static string GetContentForJobRequestNotify(string template, IEnumerable<ContractUserRole> keyPersonnelList, JobRequestEmailModel emailModel)
        {
            StringBuilder content = new StringBuilder(template);
            var keyList = "<ul>";
            foreach (var person in keyPersonnelList)
            {
                keyList += "<li>" + person.User.DisplayName + " (" + person.UserRole + ")" + "</li>";
            }
            keyList += "</li>";

            content.Replace("{RECEIVER_NAME}", emailModel.ReceipentName);
            content.Replace("{SUBMITTED_NAME}", emailModel.SubmittedBy);
            content.Replace("{CONTRACT_NUMBER}", emailModel.ContractNumber);
            content.Replace("{AWARDING_AGENCY}", emailModel.AwardingAgency);
            content.Replace("{FUNDING_AGENCY}", emailModel.FundingAgency);
            content.Replace("{TASK_ORDERNUMBER}", emailModel.TaskOrderNumber);
            content.Replace("{PROJECT_NUMBER}", emailModel.ProjectNumber);
            content.Replace("{SUBMITTED_BY}", emailModel.SubmittedBy);
            content.Replace("{STATUS}", emailModel.Status);
            content.Replace("{KEY_PERSONNELLIST}", keyList);
            content.Replace("{JOBREQUEST_URL}", emailModel.ClickableUrl);
            content.Replace("{NOTIFY_TO}", emailModel.NotifiedTo);
            content.Replace("{NOTIFY_OTHER}", emailModel.NotifyOther);
            content.Replace("{ADDITIONAL_MESSAGE}", emailModel.AdditionalMessage);
            content.Replace("{CONTRACT_TITLE}", emailModel.ContractTitle);
            content.Replace("{CONTRACT_DESCRIPTION}", emailModel.Description);
            return content.ToString();
        }
    }
}
