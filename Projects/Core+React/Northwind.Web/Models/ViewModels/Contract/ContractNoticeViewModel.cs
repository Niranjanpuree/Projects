using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Web.Models.ViewModels.Contract
{
    public class ContractNoticeViewModel 
    {
        public Guid ContractNoticeGuid { get; set; }
        public Guid ContractGuid { get; set; }
		[Required]
        [DisplayName("Notice Type")]
        public string NoticeType { get; set; }
	    public string Attachment { get; set; }
		[Required]
        public string Resolution { get; set; }
		[Required]
        [DisplayName("Issued Date")]
        public DateTime? IssuedDate { get; set; }

        public string ContractTitle   { get; set; }
        public string ProjectNumber   { get; set; }
        public string ContractNumber  { get; set; }
        [DisplayName("Notice Description")]
        public string NoticeDescription { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public string ActionItem { get; set; }

    }

    public class NoticeViewModel
    {
        public Guid ContractNoticeGuid { get; set; }
        public string NoticeType { get; set; }
        public string IssuedDate { get; set; }
        public string LastUpdatedDate { get; set; }
        public string Attachment { get; set; }
        public string Resolution { get; set; }
        public Guid ResourceGuid { get; set; }
        public string NoticeDescription { get; set; }
        public Guid UpdatedBy { get; set; }
    }
}
