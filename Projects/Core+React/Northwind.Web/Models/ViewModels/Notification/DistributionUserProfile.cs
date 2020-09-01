using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Web.Models.ViewModels
{
    public class DistributionUserProfile
    {
        public Guid UserGuid { get; set; }
        public string Username { get; set; }
        public string Lastname { get; set; }
        public string Firstname { get; set; }
        public string DisplayName { get; set; }
        public string UserStatus { get; set; }
        public string JobTitle { get; set; }
        public string Department { get; set; }
        public string WorkEmail { get; set; }
        public string PersonalEmail { get; set; }
        public bool Status { get; set; }
        public Guid DistributionListId { get; set; }
    }
}
