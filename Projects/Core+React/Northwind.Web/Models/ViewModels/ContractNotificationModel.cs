using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Web.Models.ViewModels
{
    public class ContractNotificationModel
    {
        public string ContractType { get; set; }
        public string ContractTitle { get; set; }
        public string ContractNumber { get; set; }
        public string ProjectNumber { get; set; }
        public string key { get; set; }
        public string ContractRepresentative { get; set; }
        public Guid ContractGuid { get; set; }
        public string Description { get; set; }
    }
}
