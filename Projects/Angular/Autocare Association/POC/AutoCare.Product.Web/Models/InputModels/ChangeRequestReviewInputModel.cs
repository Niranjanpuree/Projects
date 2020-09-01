using System;
using System.Collections.Generic;
using AutoCare.Product.Vcdb.Model;
using AutoCare.Product.Web.Models.ViewModels;

namespace AutoCare.Product.Web.Models.InputModels
{
    public class ChangeRequestReviewInputModel
    {
        public long ChangeRequestId { get; set; }
        public String ReviewedBy { get; set; }
        public ChangeRequestStatus ReviewStatus { get; set; }
        public CommentsStagingViewModel ReviewComment { get; set; }
        public IList<AttachmentInputModel> Attachments { get; set; }
    }
}