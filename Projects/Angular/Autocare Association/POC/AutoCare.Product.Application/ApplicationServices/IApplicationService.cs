using AutoCare.Product.Vcdb.Model;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoCare.Product.Application.Models.BusinessModels;

namespace AutoCare.Product.Application.ApplicationServices
{
    public interface IApplicationService<T>
        where T : class
    {
        Task<List<T>> GetAllAsync(int topCount = 0);
        Task<T> GetAsync<TKey>(TKey id);
        Task<List<T>> GetAsync(Expression<Func<T, bool>> whereCondition, int topCount = 0);
        Task<long> AddAsync(T entity, string requestedBy, CommentsStagingModel comment = null, List<AttachmentsModel> attachments = null);
        Task<long> UpdateAsync<TId>(T entity, TId id, string requestedBy, CommentsStagingModel comment = null, List<AttachmentsModel> attachments = null);
        Task<long> DeleteAsync<TId>(T entity,TId id, string requestedBy,CommentsStagingModel comment = null, List<AttachmentsModel> attachments = null);
        Task<int> GetCountAsync<TElement>(T entity, Expression<Func<T, ICollection<TElement>>> collectionProperty)
            where TElement : class;
        Task<List<T>> GetPendingAddChangeRequests(Expression<Func<T, bool>> whereCondition = null, int topCount = 0);
        Task<long> ReplaceAsync<TId>(T entity, TId id, string requestdBy, CommentsStagingModel comment = null, List<AttachmentsModel> attachments = null);
        Task<ChangeRequestStagingModel<T>> GetChangeRequestStaging<TId>(TId changeRequestId);
        Task<bool> SubmitChangeRequestReviewAsync<TId>(TId changeRequestId, ChangeRequestReviewModel review);
        Task<List<ChangeEntityFacet>> GetAllChangeTypes();
        //Task<AssociationCount> GetAssociatedCount(List<ChangeRequestStaging> selectedChangeRequestStagings);
        Task<bool> SubmitLikeAsync(long crId, string likedBy, string likedStatus);
    }
}
