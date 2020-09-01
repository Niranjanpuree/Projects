using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Entities
{
    public class ContractNotice
    {
        public Guid ContractNoticeGuid { get; set; }
        public string NoticeType { get; set; }
        public DateTime? IssuedDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public string Attachment { get; set; }
        public string Resolution { get; set; }
        public Guid ResourceGuid { get; set; }
        public string NoticeDescription { get; set; }
        public Guid UpdatedBy { get; set; }
    }
}
