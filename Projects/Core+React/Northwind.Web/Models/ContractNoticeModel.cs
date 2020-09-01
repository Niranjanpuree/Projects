using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Web.Models
{
    public class ContractNoticeModel
    {
        public Guid ContractNoticeGuid { get; set; }
        public string NoticeType { get; set; }
        public DateTime IssuedDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public string Attachment { get; set; }
        public string Resolution { get; set; }
        public Guid ResourceId { get; set; }

    }
}
