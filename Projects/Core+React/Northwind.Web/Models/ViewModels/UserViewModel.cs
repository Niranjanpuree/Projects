using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Web.Models.ViewModels
{
    public class UserViewModel : BaseViewModel
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
    }
}
