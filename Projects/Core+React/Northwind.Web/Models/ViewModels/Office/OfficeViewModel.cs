using Northwind.Core.Entities;
using Northwind.Core.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Web.Models.ViewModels
{
    public class OfficeViewModel : BaseViewModel
    {
        public Guid OfficeGuid { get; set; }

        [Display(Name = "Operation Manager")]
        public Guid OperationManagerGuid { get; set; }
        [Required]
        [Display(Name = "Name ")]
        public string OfficeName { get; set; }
        [Required]
        [Display(Name = "Address Line ")]
        public string PhysicalAddress { get; set; }
        [Display(Name = "Address Line 1")]
        public string PhysicalAddressLine1 { get; set; }
        [Required]
        [Display(Name = "City")]
        public string PhysicalCity { get; set; }
        [Required]
        [Display(Name = "Zip Code")]
        public string PhysicalZipCode { get; set; }
        //        [Required]
        [Display(Name = "State")]
        public Guid? PhysicalStateId { get; set; }
        [Required]
        [Display(Name = "Country")]
        public Guid PhysicalCountryId { get; set; }
        [Display(Name = "Address Line ")]
        public string MailingAddress { get; set; }
        [Display(Name = "Address Line 1")]
        public string MailingAddressLine1 { get; set; }
        [Display(Name = "City")]
        public string MailingCity { get; set; }
        [Display(Name = "Zip Code")]
        public string MailingZipCode { get; set; }
        [Display(Name = "State")]
        public Guid? MailingStateId { get; set; }
        [Display(Name = "Country")]
        public Guid? MailingCountryId { get; set; }
        [Required]
        [Display(Name = "Phone Number")]
        public string Phone { get; set; }
        //        [Required]
        [Display(Name = "Fax Number")]
        public string Fax { get; set; }
        [Required]
        [MaxLength(2, ErrorMessage = "The Code cannot be more than 2 characters !")]
        [RegularExpression(@"^[-_,A-Za-z0-9]*$", ErrorMessage = "Only alpha numeric is allowed")]
        [Display(Name = "Office Code")]
        public string OfficeCode { get; set; }
        public string StatesName { get; set; }
        public string CountryName { get; set; }

        public string PhysicalStateName { get; set; }
        public string PhysicalCountryName { get; set; }
        public string MailingStateName { get; set; }
        public string MailingCountryName { get; set; }
        public string OperationManagerName { get; set; }

        private String _physicalAddressDisplay;
        public String PhysicalAddressDisplay
        {
            get
            {
                return FormatHelper.FormatAddress(PhysicalAddress, PhysicalAddressLine1, PhysicalCity, PhysicalStateName,
                    PhysicalZipCode, PhysicalCountryName);
            }
            set { this._physicalAddressDisplay = value; }
        }

        private string _mailingAddressDisplay;
        public String MailingAddressDisplay
        {
            get
            {
                if (!string.IsNullOrEmpty(MailingStateName))
                {
                    return FormatHelper.FormatAddress(MailingAddress, MailingAddressLine1, MailingCity, MailingStateName,
                        MailingZipCode, MailingCountryName);
                }
                else
                    return null;
            }
            set { this._mailingAddressDisplay = value; }
        }
        public virtual Dictionary<Guid, string> CountrySelectListItems { get; set; }
        public virtual Dictionary<Guid, string> StatePrimarySelectListItems { get; set; }
        public virtual Dictionary<Guid, string> StateMailingSelectListItems { get; set; }
        public virtual IDictionary<Guid, string> UserSelectListItems { get; set; }
    }
}
