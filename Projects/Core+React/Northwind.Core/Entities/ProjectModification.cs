using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;
using Northwind.Core.Entities;
using Northwind.Core.Specifications;
using Northwind.Core.Utilities;

namespace Northwind.Core.Entities
{
    public class ProjectModificationModel : BaseModel
    {
        public Guid ProjectModificationGuid { get; set; }
        public Guid ProjectGuid { get; set; }
        public Guid? ContractGuid { get; set; }
        public string ContractNumber { get; set; }
        public string ModificationNumber { get; set; }
        public string ModificationTitle { get; set; }
        public DateTime? EnteredDate { get; set; }
        public DateTime? EffectiveDate { get; set; }
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
        public DateTime? POPStart { get; set; }
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
