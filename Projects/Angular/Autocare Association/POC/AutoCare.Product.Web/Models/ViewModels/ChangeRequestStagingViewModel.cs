using System;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Web.Models.ViewModels
{
    public class ChangeRequestStagingViewModel
    {
        public long Id { get; set; }
        public string Assignee { get; set; }
        public Int16? ChangeRequestTypeId { get; set; }
        public string Entity { get; set; }
        public string EntityId { get; set; }
        public string ChangeType { get; set; }
        public int TaskControllerId { get; set; }
        public string RequestedBy { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
        public ChangeRequestStatus Status { get; set; }
        public int? Likes { get; set; }
        public bool? CommentExists { get; set; }
        public string StatusText { get; set; }
        public string ChangeContent { get; set; }
    }
}