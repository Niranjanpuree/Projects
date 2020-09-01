using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Web.Models.ViewModels.Project
{
    public class ProjectModificationViewModel : BaseViewModel
    {
        public Guid ProjectModificationGuid { get; set; }
        public Guid ProjectGuid { get; set; }
        public Guid? ContractGuid { get; set; }
        public string ContractNumber { get; set; }
        //        [Required]
        [DisplayName("Modification Number")]
        public string ModificationNumber { get; set; }
        [DisplayName("Modification Title")]
        public string ModificationTitle { get; set; }
        //        [Required]
        [DisplayName("Entered Date")]
        public DateTime? EnteredDate { get; set; }
        //        [Required]
        [DisplayName("Effective Date")]
        public DateTime? EffectiveDate { get; set; }
        //        [Required]
        [DisplayName("Award Amount")]
        public decimal? AwardAmount { get; set; }
        public string FormattedAwardAmount
        {
            get
            {
                return AwardAmount == null ? "null" : string.Format("{0:C}", AwardAmount.Value);
            }
        }
        public decimal? FundingAmount { get; set; }
        public string FormattedFundingAmount
        {
            get
            {
                return FundingAmount == null ? "null" : string.Format("{0:C}", FundingAmount.Value);
            }
        }
        //        [Required]
        [Display(Name = "Period of Performance Start")]
        public DateTime? POPStart { get; set; }
        //        [Required]
        [Display(Name = "Period of Performance End")]
        public DateTime? POPEnd { get; set; }
        public List<IFormFile> FileToUpload { get; set; }
        private string _FileName;
        public string FileName
        {
            get { return !string.IsNullOrEmpty(UploadedFileName) ? UploadedFileName.Split("\\").Last() : String.Empty; }
            set { _FileName = value; }
        }
        public string UploadedFileName { get; set; }
        public string Description { get; set; }
        public string ProjectNumber { get; set; }
        public string ProjectTitle { get; set; }
        public bool IsAwardAmount { get; set; }
        public bool IsFundingAmount { get; set; }
        public bool IsPOP { get; set; }
    }
}
