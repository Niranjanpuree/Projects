using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoCare.Product.Infrastructure.RepositoryService;
using AutoCare.Product.Infrastructure.Serializer;
using AutoCare.Product.Vcdb.Model;
using AutoMapper;
using System.Linq.Expressions;
using AutoCare.Product.Application.Models.BusinessModels;
using AutoCare.Product.VcdbSearch.Indexing.ApplicationService;

namespace AutoCare.Product.Application.BusinessServices
{
    public abstract class ChangeRequestBusinessService<T, TItem, TComment, TAttachment> : ChangeRequestStagingBase<T>, IChangeRequestBusinessService<T, TItem, TComment, TAttachment>
        where T : class
        where TItem : class
        where TComment : class
        where TAttachment : class
    {
        private readonly IRepositoryService<T> _changeRequestRepositoryService;
        protected readonly IMapper AutoMapper;
        private readonly IChangeRequestItemBusinessService<TItem> _changeRequestItemBusinessService;

        protected ChangeRequestBusinessService(IUnitOfWork repositories, IMapper autoMapper, ITextSerializer serializer,
            IChangeRequestItemBusinessService<TItem> changeRequestItemBusinessService,
            IChangeRequestCommentsBusinessService<TComment> changeRequestCommentsBusinessService,
            IChangeRequestIndexingService changeRequestIndexingService) : base(repositories, serializer)
        {
            AutoMapper = autoMapper;
            _changeRequestItemBusinessService = changeRequestItemBusinessService;
            _changeRequestRepositoryService = repositories.GetRepositoryService<T>();
        }

        public virtual async Task<List<T>> GetChangeRequestsAsync(Expression<Func<T, bool>> whereCondition,
            int topCount = 0)
        {
            return await this._changeRequestRepositoryService.GetAsync(whereCondition, topCount);
        }

        public virtual async Task<long> ChangeRequestExist<TEntity, TId>(TEntity entity, TId id)
        {
            return await this._changeRequestItemBusinessService.ChangeRequestItemExistAsync(entity, id);
        }

        public virtual async Task<long> ChangeRequestExist<TId>(string entityName, TId id)
        {
            return await this._changeRequestItemBusinessService.ChangeRequestItemExistAsync(entityName, id);
        }

        public virtual async Task<long> ChangeRequestExist<TEntity>(Expression<Func<TEntity, bool>> whereCondition)
        {
            return await this._changeRequestItemBusinessService.ChangeRequestItemExistAsync(whereCondition);
        }

        public abstract Task<bool> DeleteAsync<TKey>(TKey changeRequestId, string deletedBy,
            TComment commentsStaging = null, List<TAttachment> attachments = null);

        public abstract Task<bool> ApproveAsync<TKey>(TKey changeRequestId, string approvedBy,
            TComment commentsStaging = null, List<TAttachment> attachments = null);

        public abstract Task<bool> RejectAsync<TKey>(TKey changeRequestId, string rejectedBy,
            TComment commentsStaging = null, List<TAttachment> attachments = null);

        public abstract Task<bool> PreliminaryApproveAsync<TId>(TId changeRequestId, string approvedBy,
            TComment commentsStaging = null, List<TAttachment> attachments = null);

        public abstract Task<bool> SubmitAsync<TKey>(TKey changeRequestId, string submittedBy, TComment commentsStaging = default(TComment),
            List<TAttachment> attachments = null);

        public abstract Task<bool> SubmitLikeAsync(long crId, string likedBy, string likedStatus);

        public abstract Task<long> SubmitAsync<TEntity, TId>(TEntity entity, TId id, string requestedBy,
            ChangeType changeType = ChangeType.None,
            List<TItem> changeRequestItemStagings = null, TComment changeRequestCommentsStaging = null,
            List<AttachmentsStaging> attachmentsStaging = null, string changeContent = null);

        public virtual async Task<bool> SubmitReviewAsync<TId>(TId changeRequestId, string reviewedBy, ChangeRequestStatus reviewStatus,
            TComment commentsStaging = null, List<TAttachment> attachments = null)
        {
            bool result = false;
            // check review status
            switch (reviewStatus)
            {
                case ChangeRequestStatus.Submitted: // never executed
                    break;
                case ChangeRequestStatus.Deleted: // if review status .equals cancel
                    result = await this.DeleteAsync(changeRequestId, reviewedBy, commentsStaging);
                    break;
                case ChangeRequestStatus.PreliminaryApproved: // if review status .equals preliminary approve
                    result = await this.PreliminaryApproveAsync(changeRequestId, reviewedBy, commentsStaging);
                    break;
                case ChangeRequestStatus.Approved: // if review status .equals approve
                    result = await this.ApproveAsync(changeRequestId, reviewedBy, commentsStaging);
                    break;
                case ChangeRequestStatus.Rejected:  // if review status .equals reject
                    result = await this.RejectAsync(changeRequestId, reviewedBy, commentsStaging);
                    break;
            }

            return result;
        }

        public virtual async Task<List<TItem>> GetChangeRequestItemStagingsAsync(
            Expression<Func<TItem, bool>> whereCondition, int topCount = 0)
        {
            return await this._changeRequestItemBusinessService.GetChangeRequestItemsAsync(whereCondition, topCount);
        }

        public virtual async Task<List<T>> GetAllChangeRequestsAsync(int topCount = 0)
        {
            return await _changeRequestRepositoryService.GetAllAsync(topCount);
        }

        public abstract Task<ChangeRequestStagingModel<TEntity>> GetChangeRequestStagingByChangeRequestIdAsync
            <TEntity, TId>(TId changeRequestId)
            where TEntity : class;

        public abstract Task<ChangeRequestStagingModel<TEntity>> GetChangeRequestStoreByChangeRequestIdAsync<TEntity, TId>(
            TId changeRequestId) where TEntity : class;

        public abstract Task<List<ChangeEntityFacet>> GetAllChangeTypes();

        public abstract Task<AssociationCount> GetAssociatedCount(List<ChangeRequestStaging> selectedChangeRequestStagings);

        public abstract Task<bool> AssignReviewer(AssignReviewerBusinessModel assignReviewerBusinessModel);

        public abstract Task<List<CommentsStagingModel>> GetRequestorComments(string changeRequestId, string reviewStatus);
    }
}
