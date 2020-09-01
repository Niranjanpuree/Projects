using System;

namespace AutoCare.Product.Vcdb.Model
{
    public class CommentsStaging
    {
        public long Id{ get; set; }
        public long ChangeRequestId { get; set; }
        public string AddedBy { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedDatetime { get; set; }

        public ChangeRequestStaging ChangeRequest { get; set; }
    }
}
