using Northwind.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Web.Infrastructure.Models.ViewModels
{
    public class UserViewModel
    {
        public Guid UserGuid { get; set; }
        public string Username { get; set; }
        public string Lastname { get; set; }
        public string Firstname { get; set; }
        public string Givenname { get; set; }
        public string DisplayName { get; set; }
        public string UserStatus { get; set; }
        public string WorkEmail { get; set; }
        public string PersonalEmail { get; set; }
        public string WorkPhone { get; set; }
        public string HomePhone { get; set; }
        public string MobilePhone { get; set; }
        public string JobTitle { get; set; }
        public string Company { get; set; }
        public string Department { get; set; }
        public string Extension { get; set; }
        public string JobStatus { get; set; }
        public string Manager { get; set; }
        public string Group { get; set; }
        public string Role { get; set; }
        public bool IsActive { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDeleted { get; set; }
        public string SearchValue { get; set; }
        public string IsActiveStatus { get { return IsActive == true ? EnumGlobal.ActiveStatus.Active.ToString() : EnumGlobal.ActiveStatus.Inactive.ToString(); } }
    }
}
