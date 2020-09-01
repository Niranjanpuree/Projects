using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Northwind.Core.Specifications;
using Northwind.Core.Utilities;

namespace Northwind.Web.Models.ViewModels.Customer
{
    public class CustomerViewModel : BaseViewModel
    {
        public Guid CustomerGuid { get; set; }
        [Required]
        [Display(Name = "Name")]
        public string CustomerName { get; set; }
        public string DisplayName { get; set; }
        [Display(Name = "Address Line 1")]
        public string AddressLine1 { get; set; }
        [Required]
        [Display(Name = "City")]
        public string City { get; set; }
        [Required]
        [Display(Name = "Zip Code")]
        public string ZipCode { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        [Display(Name = "Customer Type")]
        public string CustomerTypeName { get; set; }
        [Display(Name = "Abbreviations")]
        public string Abbreviations { get; set; }
        [Display(Name = "Tags")]
        public string Tags { get; set; }
        public SearchVM SearchVm { get; set; }
        [Display(Name = "Agency")]
        public string Agency { get; set; }
        [Display(Name = "Primary Email")]
        public string PrimaryEmail { get; set; }
        [Display(Name = "Department")]
        public string Department { get; set; }
        [Display(Name = "Code")]
        [RegularExpression(@"^[-_,A-Za-z0-9]*$", ErrorMessage = "Only alpha numeric is allowed")]
        public string CustomerCode { get; set; }
        [Required]
        public string Address { get; set; }

        [Display(Name = "State")]
        public Guid? StateId { get; set; }
        [Required]
        [Display(Name = "Country")]
        public Guid CountryId { get; set; }
        [Required]
        [Display(Name = "Customer Type")]
        public Guid CustomerTypeGuid { get; set; }
        [Display(Name = "Customer Description")]
        public string CustomerDescription { get; set; }

        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public string PrimaryPhone { get; set; }

        [Display(Name = "Url")]
        public string Url { get; set; }
        public string StatesName { get; set; }
        public string CountryName { get; set; }

        public IDictionary<Guid, string> StateSelectListItems { get; set; }
        public IDictionary<string, string> AgencySelectListItems { get; set; }
        public IDictionary<string, string> DepartmentSelectListItems { get; set; }
        public IDictionary<Guid, string> CustomerTypeSelectListItems { get; set; }
        public IDictionary<Guid, string> CountrySelectListItems { get; set; }
    }
}
