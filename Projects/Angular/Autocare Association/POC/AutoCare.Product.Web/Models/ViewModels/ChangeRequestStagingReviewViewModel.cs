using System;

namespace AutoCare.Product.Web.Models.ViewModels
{
    public class ChangeRequestStagingReviewViewModel
    {
        public long ChangeRequestId { get; set; }
        public long ChangeRequestItemId { get; set; }
        public string EntityName { get; set; }
        public string EntityId { get; set; }
        public String ChangeType { get; set; }
        public String Status { get; set; }
        public string SubmittedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
    }
}