using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Northwind.Web.Models.ViewModels
{
    public class OfficeContactViewModel : BaseViewModel
    {
        public Guid ContactGuid { get; set; }
        [Required]
        [Display(Name = "Office")]
        public Guid OfficeGuid { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }


        [Required]
        [Display(Name = "Contact Type")]
        public Guid ContactType { get; set; }

        [Required]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Alternative Number")]
        public string AltPhoneNumber { get; set; }

        //[Required]
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }
        [Display(Name = "Alternative Email")]
        public string AltEmailAddress { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }
        [Display(Name = "Address")]
        public string Address { get; set; }
        public string FullName { get; set; }
        public string ContactNumber { get; set; }
        public string ContactTypeName { get; set; }
        [Display(Name = "Office Name")]
        public string OfficeName { get; set; }

        public virtual IDictionary<Guid, string> ContactTypeSelectListItems { get; set; }
    }
}
