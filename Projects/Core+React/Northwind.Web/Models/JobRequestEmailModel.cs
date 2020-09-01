using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Web.Models
{
    public class JobRequestEmailModel
    {
        public string ContractNumber { get; set; }
        public string ProjectNumber { get; set; }
        public string AwardingAgency { get; set; }
        public string FundingAgency { get; set; }
        public string TaskOrderNumber { get; set; }
        public string SubmittedBy { get; set; }
        public string Status { get; set; }
        public string ClickableUrl { get; set; }
        public string CopiedTo { get; set; }
        public string NotifiedTo { get; set; }
        public string NotifyOther { get; set; }
        public string ReceipentName { get; set; }
        public string SubmittedName { get; set; }
        public string AdditionalMessage { get; set; }
        public string ContractTitle { get; set; }
        public string Description { get; set; }
    }
}
