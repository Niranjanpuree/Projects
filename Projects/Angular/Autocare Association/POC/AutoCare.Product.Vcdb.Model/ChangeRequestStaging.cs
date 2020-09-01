using System;
using System.Collections.Generic;

namespace AutoCare.Product.Vcdb.Model
{
    public class ChangeRequestStaging
    {
        public long Id { get; set; }
        public Int16 ChangeRequestTypeId { get; set; }
        public string Entity { get; set; }
        public string EntityId { get; set; }
        public ChangeType ChangeType { get; set; }
        public int? TaskControllerId { get; set; }
        public string RequestedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public ChangeRequestStatus Status { get; set; }

        public ICollection<ChangeRequestItemStaging> ChangeRequestItemStagings { get; set; }
        public ICollection<CommentsStaging> CommentsStagings { get; set; }
        public ICollection<AttachmentsStaging> AttachmentsStagings { get; set; } 
        public TaskController TaskController { get; set; }
        public ICollection<LikesStaging> LikesStagings { get; set; }
        public ICollection<ChangeRequestAssignmentStaging> ChangeRequestAssignmentStagings { get; set; }
    }
}
