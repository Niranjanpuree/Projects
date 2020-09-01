using System.Collections.Generic;
using System.Threading.Tasks;
using AutoCare.Product.Application.BusinessServices;
using System;
using System.Linq.Expressions;
using AutoCare.Product.Application.Models.BusinessModels;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.ApplicationServices
{
    public abstract class ApplicationService<T> : IApplicationService<T>
        where T : class
    {
        private readonly IBusinessService<T> _businessService;

        protected ApplicationService(IBusinessService<T> businessService)
        {
            _businessService = businessService;
        }

        public virtual async Task<List<T>> GetAllAsync(int topCount = 0)
        {
            return await _businessService.GetAllAsync(topCount);
        }

        public virtual async Task<T> GetAsync<TKey>(TKey id)
        {
            return await _businessService.GetAsync(id);
        }

        public virtual async Task<List<T>> GetAsync(Expression<Func<T, bool>> whereCondition, int topCount = 0)
        {
            return await _businessService.GetAsync(whereCondition, topCount);
        }

        public abstract Task<long> AddAsync(T entity, string requestedBy, CommentsStagingModel comment = null, List<AttachmentsModel> attachments = null);

        public abstract Task<long> UpdateAsync<TId>(T entity, TId id, string requestedBy,
            CommentsStagingModel comment = null, List<AttachmentsModel> attachments = null);

        public abstract Task<long> DeleteAsync<TId>(T entity, TId id, string requestedBy,
            CommentsStagingModel comment = null, List<AttachmentsModel> attachments = null);

        public abstract Task<long> ReplaceAsync<TId>(T entity, TId id, string requestdBy,
            CommentsStagingModel comment = null, List<AttachmentsModel> attachments = null);


        public async Task<int> GetCountAsync<TElement>(T entity,
            Expression<Func<T, ICollection<TElement>>> collectionProperty) where TElement : class
        {
            return await _businessService.GetCountAsync(entity, collectionProperty);
        }

        public async Task<List<T>> GetPendingAddChangeRequests(Expression<Func<T, bool>> whereCondition = null, int topCount = 0)
        {
            return await _businessService.GetPendingAddChangeRequests(whereCondition, topCount);
        }

        public virtual async Task<bool> RemoveAsync(int id)
        {
            return await _businessService.RemoveAsync(id);
        }
        
        public async Task<ChangeRequestStagingModel<T>> GetChangeRequestStaging<TId>(TId changeRequestId)
        {
            return await _businessService.GetChangeRequestStaging(changeRequestId);
        }

        public async Task<bool> SubmitChangeRequestReviewAsync<TId>(TId changeRequestId, ChangeRequestReviewModel review)
        {
            return await _businessService.SubmitChangeRequestReviewAsync(changeRequestId, review);
        }

        public virtual async Task<List<ChangeEntityFacet>> GetAllChangeTypes()
        {
            return await _businessService.GetAllChangeTypes();
        }

        public virtual async Task<bool> SubmitLikeAsync(long crId, string likedBy, string likedStatus)
        {
            return await _businessService.SubmitLikeAsync(crId, likedBy, likedStatus);
        }

        
    }
}
