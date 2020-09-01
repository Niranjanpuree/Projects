using System.Collections.Generic;
using System.Threading.Tasks;
using AutoCare.Product.Vcdb.Model;
using System;
using System.Linq.Expressions;
using AutoCare.Product.Application.Models.BusinessModels;

namespace AutoCare.Product.Application.BusinessServices
{
    public interface IChangeRequestBusinessService<T, TItem, TComment, TAttachment> : IChangeRequestStagingBase<T>
        where T:class
        where TItem: class
        where TComment : class
        where TAttachment: class
    {
        Task<List<T>> GetAllChangeRequestsAsync(int topCount = 0);
        Task<List<T>> GetChangeRequestsAsync(Expression<Func<T, bool>> whereCondition, int topCount = 0);

        Task<long> ChangeRequestExist<TEntity, TId>(TEntity entity, TId id);
        Task<long> ChangeRequestExist<TId>(string entityName, TId id);
        Task<long> ChangeRequestExist<TEntity>(Expression<Func<TEntity, bool>> whereCondition);

        Task<bool> DeleteAsync<TKey>(TKey changeRequestId, string deletedBy,
            TComment commentsStaging = null, List<TAttachment> attachments = null);
        Task<bool> ApproveAsync<TKey>(TKey changeRequestId, string approvedBy,
            TComment commentsStaging = null, List<TAttachment> attachments = null);
        Task<bool> RejectAsync<TKey>(TKey changeRequestId, string rejectedBy,
            TComment commentsStaging = null, List<TAttachment> attachments = null);
        Task<bool> PreliminaryApproveAsync<TId>(TId changeRequestId, string approvedBy,
            TComment commentsStaging = null, List<TAttachment> attachments = null);
        Task<bool> SubmitAsync<TKey>(TKey changeRequestId, string submittedBy,
           TComment commentsStaging = null, List<TAttachment> attachments = null);

        Task<bool> SubmitLikeAsync(long crId, string likedBy, string likedStatus);

        Task<long> SubmitAsync<TEntity, TId>(TEntity entity, TId id, string requestedBy,
            ChangeType changeType = ChangeType.None,
            List<TItem> changeRequestItemStagings = null,
            TComment changeRequestCommentsStaging = null,
            List<AttachmentsStaging> attachmentsStaging = null,
            string changeContent = null);
        Task<bool> SubmitReviewAsync<TId>(TId changeRequestId, string reviewedBy, ChangeRequestStatus reviewStatus,
            TComment commentsStaging = null, List<TAttachment> attachments = null);

        Task<List<TItem>> GetChangeRequestItemStagingsAsync(Expression<Func<TItem, bool>> whereCondition,
            int topCount = 0);
        Task<ChangeRequestStagingModel<TEntity>> GetChangeRequestStagingByChangeRequestIdAsync<TEntity, TId>(
            TId changeRequestId) where TEntity : class;
        Task<ChangeRequestStagingModel<TEntity>> GetChangeRequestStoreByChangeRequestIdAsync<TEntity, TId>
            (TId changeRequestId) where TEntity : class;

        Task<List<ChangeEntityFacet>> GetAllChangeTypes();
        Task<AssociationCount> GetAssociatedCount(List<ChangeRequestStaging> selectedChangeRequestStagings);
        Task<bool> AssignReviewer(AssignReviewerBusinessModel assignReviewerBusinessModel);
        Task<List<CommentsStagingModel>> GetRequestorComments(string changeRequestId, string reviewStatus);

    }
}
