using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Northwind.Core.Entities
{
   public class CustomerContact:BaseModel
    {
        [Required]
        public Guid ContactGuid { get; set; }
        [Required]
        [Display (Name ="Customer")]
        public Guid CustomerGuid { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        //[Required]
        //[Display(Name = "Gender")]
        //public string Gender { get; set; }

        [Required]
        [Display(Name = "Contact Type")]
        public Guid ContactTypeGuid { get; set; }

        //[Required]
        //[Display(Name = "Job Title")]
        //public string JobTitle { get; set; }

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

        [Display(Name = "Notes")]
        public string Notes { get; set; }

        public string FullName { get; set; }
        public string ContactNumber { get; set; }
        public string ContactTypeName { get; set; }
        [Display(Name = "Customer Name")]
        public string CustomerName { get; set; }
    }
}
