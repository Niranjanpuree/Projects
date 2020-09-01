using System;
using  System.Collections.Generic;

namespace AutoCare.Product.Vcdb.Model
{
    public class ChangeRequestStore
    {
        public long Id { get; set; }
        public Int16 ChangeRequestTypeId { get; set; }
        public string Entity { get; set; }
        public string EntityId { get; set; }
        public ChangeType ChangeType { get; set; }
        public ChangeRequestStatus Status { get; set; }
        public string DecisionBy { get; set; }
        public int? TaskControllerId { get; set; }
        public string RequestedBy { get; set; }
        public DateTime RequestedDateTime { get; set; }
        public DateTime CompletedDateTime { get; set; }

        public ICollection<ChangeRequestItem> ChangeRequestItems { get; set; }
        public ICollection<Comments> Comments { get; set; }
        public ICollection<Attachments> Attachments { get; set; }
        public TaskController TaskController { get; set; }
        public ICollection<Likes> Likes { get; set; }

        public ICollection<ChangeRequestAssignment> ChangeRequestAssignments { get; set; }
    }
}
