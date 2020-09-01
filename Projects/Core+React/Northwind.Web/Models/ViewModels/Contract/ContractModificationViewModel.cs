using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Web.Models.ViewModels.Contract
{
    public class ContractModificationViewModel : BaseViewModel
    {
        public Guid ContractModificationGuid { get; set; }
        public Guid ContractGuid { get; set; }
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
        public string Currency { get; set; }

        [DisplayName("Award Amount")]
        public decimal? AwardAmount { get; set; }
        public string FormattedAwardAmount
        {
            get
            {
                return AwardAmount == null ? "Not Entered" : Currency + " " + String.Format("{0:###,##0.00}", AwardAmount.Value);
            }
        }


        [DisplayName("Funded Amount")]
        public decimal? FundingAmount { get; set; }
        public string FormattedFundingAmount
        {
            get
            {
                return FundingAmount == null ? "Not Entered" : Currency + " " + String.Format("{0:###,###.00}", FundingAmount.Value);
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
        [DisplayName("Description")]
        public string Description { get; set; }
        public string ContractNumber { get; set; }
        public string ContractTitle { get; set; }
        public string ProjectNumber { get; set; }
        public bool IsAwardAmount { get; set; }
        public bool IsFundingAmount { get; set; }
        public bool IsPOP { get; set; }
        [DisplayName("Modification Type")]
        public string ModificationType { get; set; }

        public Dictionary<string,string> keyValuePairs { get; set; }
    }
}
