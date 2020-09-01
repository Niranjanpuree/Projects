using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Northwind.Core.Utilities;

namespace Northwind.Web.Areas.IAM.Models.IAM.ViewModels
{
    public class UserViewModel
    {
        public Guid UserGuid { get; set; }
        public string Name { get { return Firstname + " " + Lastname; } set { } }

        [Required(ErrorMessage = "Please enter password.")]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please enter confirm password.")]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Password doesn't match with confirm password.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Please enter username.")]
        [Display(Name = "Username")]
        public string Username { get; set; }

        private String _displayName;
        [Display(Name = "Display Name")]
        public String Displayname
        {
            get
            {
                return string.IsNullOrEmpty(this._displayName) ? FormatHelper.FormatFullName(Firstname, string.Empty, Lastname) : this._displayName;
            }
            set { this._displayName = value; }
        }

        [Required(ErrorMessage = "Please enter last name.")]
        [Display(Name = "Lastname")]
        public string Lastname { get; set; }

        [Required(ErrorMessage = "Please enter first name.")]
        [Display(Name = "Firstname")]
        public string Firstname { get; set; }
        [Display(Name = "Given Name")]
        public string Givenname { get; set; }
        //        [Display(Name = "Display Name")]
        //        public string DisplayName { get; set; }

        [Required(ErrorMessage = "Please select an status.")]
        [Display(Name = "Status")]
        public string UserStatus { get; set; }

        [Required(ErrorMessage = "Please enter work email address.")]
        [EmailAddress(ErrorMessage = "Please enter valid email address.")]
        [Display(Name = "Work Email")]
        public string WorkEmail { get; set; }
        [Display(Name = "Personal Email")]
        public string PersonalEmail { get; set; }
        [Display(Name = "Phone")]
        [Required(AllowEmptyStrings = true)]
        public string WorkPhone { get; set; }
        [Display(Name = "Home Phone")]
        [Required(AllowEmptyStrings = true)]
        public string HomePhone { get; set; }
        [Display(Name = "Mobile")]
        public string MobilePhone { get; set; }
        [Required(ErrorMessage = "Please select job title.")]
        [Display(Name = "Job Title")]
        public string JobTitle { get; set; }
        public string Company { get; set; }
        public string Department { get; set; }
        public string Extension { get; set; }
        [Display(Name = "Job Status")]
        public string JobStatus { get; set; }
        public string Manager { get; set; }
        public string Group { get; set; }
    }
}
