using Northwind.Web.Models.ViewModels.Company;
using Northwind.Web.Models.ViewModels.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Northwind.Web.Models.ViewModels.Region

{
    public class RegionViewModel : BaseViewModel
    {
        [Required]
        [Display(Name = "Region Guid")]
        public Guid RegionGuid { get; set; }
        [Required]
        [Display(Name = "Name")]
        public string RegionName { get; set; }
        [Required]
        [RegularExpression(@"^[-_,A-Za-z0-9]*$", ErrorMessage = "Only alpha numeric is allowed")]
        [Display(Name = "Code")]
        public string RegionCode { get; set; }
        [Required]
        [Display(Name = "Regional Manager")]
        public Guid RegionalManager { get; set; }
        [Display(Name = "Manager Name")]
        public string ManagerName { get; set; }
        [Required]
        [Display(Name = "Deputy Regional Manager")]
        public Guid DeputyRegionalManager { get; set; }
        [Display(Name = "Deputy Regional Manager Name")]
        public string DeputyManagerName { get; set; }
        [Required]
        [Display(Name = "Business Development Regional Manager")]
        public Guid BusinessDevelopmentRegionalManager { get; set; }
        [Display(Name = "Business Development Manager Name")]
        public string BDManagerName { get; set; }
        [Required]
        [Display(Name = "Health and Services Regional Manager")]
        public Guid HSRegionalManager { get; set; }
        [Display(Name = "Manager Name")]
        public string HSManagerName { get; set; }
        public IDictionary<Guid, string> UserSelectListItems { get; set; }
       
    }
}
