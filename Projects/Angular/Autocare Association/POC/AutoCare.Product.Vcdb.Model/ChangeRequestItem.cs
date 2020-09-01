using System;

namespace AutoCare.Product.Vcdb.Model
{
    public class ChangeRequestItem
    {
        public long ChangeRequestItemId { get; set; }
        public long ChangeRequestId { get; set; }
        public string Entity { get; set; }
        public string EntityFullName { get; set; }
        public string EntityId { get; set; }
        public string Payload { get; set; }
        public string ExistingPayload { get; set; }
        public ChangeType ChangeType { get; set; }
        public DateTime CreatedDateTime { get; set; }

        public ChangeRequestStore ChangeRequestStore { get; set; }
    }
}
