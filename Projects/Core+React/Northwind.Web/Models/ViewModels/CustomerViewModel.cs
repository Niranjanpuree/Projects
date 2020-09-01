using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Web.Models.ViewModels
{
    public class CustomerViewModel
    {
        public Guid CustomerGuid { get; set; }
        [Required]
        [Display(Name = "Name ")]
        public string CustomerName { get; set; }
        [Required]
        [Display(Name = "Address Line")]
        public string Address { get; set; }
        [Display(Name = "Address Line 1")]
        public string AddressLine1 { get; set; }
        [Required]
        [Display(Name = "City")]
        public string City { get; set; }
        [Required]
        [Display(Name = "Zip Code")]
        public string ZipCode { get; set; }
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

        [Display(Name = "Primary Email")]
        //[EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string PrimaryEmail { get; set; }

        [Display(Name = "Abbreviations")]
        public string Abbreviations { get; set; }
        [Display(Name = "Tags")]
        public string Tags { get; set; }
        [Display(Name = "Url")]
        public string Url { get; set; }
        [Display(Name = "Department")]
        public string Department { get; set; }
        [Display(Name = "Code")]
        public string CustomerCode { get; set; }
        [Display(Name = "Agency")]
        public string Agency { get; set; }
        [Display(Name = "Customer Type")]
        public string CustomerTypeName { get; set; }
        public string StatesName { get; set; }
        public string CountryName { get; set; }

        public virtual ICollection<KeyValuePairWithDescriptionModel<Guid, string, bool>> StateSelectListItems { get; set; }
        public virtual ICollection<KeyValuePairModel<string, string>> AgencySelectListItems { get; set; }
        public virtual ICollection<KeyValuePairModel<string, string>> DepartmentSelectListItems { get; set; }
        public virtual ICollection<KeyValuePairModel<Guid, string>> CustomerTypeSelectListItems { get; set; }
        public virtual ICollection<KeyValuePairModel<Guid, string>> CountrySelectListItems { get; set; }
    }
}
