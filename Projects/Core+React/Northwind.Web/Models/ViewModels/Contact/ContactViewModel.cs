using Northwind.Web.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Northwind.Web.Models.ViewModels.Contact
{
    public class ContactViewModel:BaseViewModel
    {
        [Display(Name="Contract Guid")]
        public Guid ContactGuid { get; set; }
        [Display(Name = "Customer Guid")]
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
        [Display(Name = "Contact Type")]
        public Guid ContactType { get; set; }
        //[Required]
        //[Display(Name = "Job Title")]
        //public string JobTitle { get; set; }
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Alternative Phone Number")]
        public string AltPhoneNumber { get; set; }
        [Display(Name = "Alternative Email")]
        public string AltEmailAddress { get; set; }
        [Display(Name = "Notes")]
        public string Notes { get; set; }
        [Display(Name = "Full Name")]
        public string FullName { get; set; }
        public string ContactsearchValue { get; set; }
        [Display(Name = "Contact Type")]
        public string ContactTypeName { get; set; }
        [Display(Name = "Customer Name")]
        public string CustomerName { get; set; }
        [Display(Name ="Email Address")]
        public string EmailAddress { get; set; }

        private String _PhoneNumber;
        private String _EmailAddress;
        [Display(Name = "Contact Number")]
        public String ContactNumbers
        {
            get
            {
                return FormatHelper.ConcatTwoString(PhoneNumber, AltPhoneNumber);
            }
            set { this._PhoneNumber = value; }
        }
        [Display(Name = "Email Address")]
        public String EmailAddresses
        {
            get
            {
                return FormatHelper.ConcatTwoString(EmailAddress, AltEmailAddress);
            }
            set { this._EmailAddress = value; }
        }
        [Display(Name = "Office Guid")]
        public Guid OfficeGuid { get; set; }
        [Display(Name = "Office Name")]
        public string OfficeName { get; set; }

        public IDictionary<Guid, string> ContactTypeSelectListItems { get; set; }
        public IDictionary<string, string> GenderSelectListItems { get; set; }
    }
}
