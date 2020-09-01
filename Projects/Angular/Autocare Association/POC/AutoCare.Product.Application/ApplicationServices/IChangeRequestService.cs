using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoCare.Product.Application.Models.BusinessModels;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.ApplicationServices
{
    public interface IChangeRequestService<T, TItem, TComment, TAttachment>
        where T: class
        where TItem: class
        where TComment: class
        where TAttachment : class
    {
        Task<long> SubmitAsync<TId>(T entity, TId id, string requestedBy);
        Task<bool> ApproveAsync<TKey>(TKey changeRequestId, string approvedBy);
        Task<bool> RejectAsync<TKey>(TKey changeRequestId, string rejectedBy);
        Task<List<T>> GetAllChangeRequestsAsync(int topCount = 0);
        Task<List<T>> GetChangeRequestsAsync(Expression<Func<T, bool>> whereCondition, int topCount = 0);
        Task<List<TItem>> GetChangeRequestItemStagingsAsync(Expression<Func<TItem, bool>> whereCondition,
            int topCount = 0);
        Task<List<ChangeEntityFacet>> GetAllChangeTypes();
        Task<AssociationCount> GetAssociatedCount(List<ChangeRequestStaging> selectedChangeRequestStagings);
        Task<bool> AssignReviewer(AssignReviewerBusinessModel assignReviewerBusinessModel);
        Task<List<CommentsStagingModel>> GetRequestorComments(string changeRequestId,string reviewStatus);
    }
}
