using System;
using Microsoft.Azure.Search.Models;

namespace AutoCare.Product.VcdbSearchIndex.Model
{
    [SerializePropertyNamesAsCamelCase]
    public class ChangeRequestDocument
    {
        public string ChangeRequestId { get; set; }
        public Int16? ChangeRequestTypeId { get; set; }
        public short? Status { get; set; }
        public string StatusText { get; set; }
        public string RequestedBy { get; set; }
        public string ChangeType { get; set; }
        public DateTime? SubmittedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string Source { get; set; }
        public int? Likes { get; set; }
        public string Entity { get; set; }
        public string Assignee { get; set; }
        public bool? CommentExists { get; set; }
        public string ChangeContent { get; set; }
    }
}
