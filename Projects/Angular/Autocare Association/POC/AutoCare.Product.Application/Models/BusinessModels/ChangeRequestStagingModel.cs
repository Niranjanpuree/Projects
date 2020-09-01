using System;
using System.Collections.Generic;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.Models.BusinessModels
{
    public class ChangeRequestStagingModel<TEntity> where TEntity:class
    {
        public ChangeRequestStagingReviewModel StagingItem { get; set; }
        public TEntity EntityStaging { get; set; }
        public TEntity EntityCurrent { get; set; }
        //public IList<CommentsStagingModel> RequestorComments { get; set; }
        //public IList<CommentsStagingModel> ReviewerComments { get; set; }
        public IList<CommentsStagingModel> Comments { get; set; }
        public IList<AttachmentsModel> Attachments { get; set; }
    }

    public class ChangeRequestStagingReviewModel
    {
        public long ChangeRequestId { get; set; }
        public string EntityName { get; set; }
        public string EntityId { get; set; }
        public String ChangeType { get; set; }
        public String Status { get; set; }
        public string SubmittedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
    }

    public class ChangeRequestReviewModel
    {
        public long ChangeRequestId { get; set; }
        public String ReviewedBy { get; set; }
        public ChangeRequestStatus ReviewStatus { get; set; }
        public CommentsStagingModel ReviewComment { get; set; }
        public List<AttachmentsModel> Attachments { get; set; }
    }
}
