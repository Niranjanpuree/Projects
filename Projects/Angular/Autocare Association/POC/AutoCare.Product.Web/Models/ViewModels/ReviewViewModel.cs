using System.Collections.Generic;

namespace AutoCare.Product.Web.Models.ViewModels
{
    public class ReviewViewModel
    {
        public bool CanAttach { get; set; }
        public bool CanDelete { get; set; }
        public bool CanLike { get; set; }
        public bool CanAssign { get; set; }
        public bool CanReview { get; set; }
        public bool CanFinal { get; set; }
        public bool CanSubmit { get; set; }
        public bool IsCompleted { get; set; }

        public ChangeRequestStagingReviewViewModel StagingItem;
         public IList<AttachmentsStagingViewModel> Attachments { get; set; }
    }
}