using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Northwind.Web.Models.ViewModels.Company
{
    public class CompanyViewModel : BaseViewModel
    {
        [Required]
        [Display(Name = "Company Guid")]
        public Guid CompanyGuid { get; set; }
        [Required]
        [Display(Name = "Name")]
        public string CompanyName { get; set; }
        [Required]
        [MaxLength(2, ErrorMessage = "The Code cannot be more than 2 characters !")]
        [RegularExpression(@"^[-_,A-Za-z0-9]*$", ErrorMessage = "Only alpha numeric is allowed")]
        [Display(Name = "Code")]
        public string CompanyCode { get; set; }
        [Required]
        [Display(Name = "President")]
        public Guid President { get; set; }
        public string PresidentName { get; set; }
        [Required]
        [Display(Name = "Abbreviation")]
        public string Abbreviation { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }
        public IDictionary<Guid, string> UserSelectListItems { get; set; }
    }
}
