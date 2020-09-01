using System;

namespace AutoCare.Product.Vcdb.Model
{
    public class Comments
    {
        public long Id { get; set; }
        public long ChangeRequestId { get; set; }
        public string AddedBy { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedDatetime { get; set; }

        public ChangeRequestStore ChangeRequestStore { get; set; }
    }
}
