using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoCare.Product.Application.Models.BusinessModels;
using AutoCare.Product.Vcdb.Model;
using AutoCare.Product.Application.BusinessServices;

namespace AutoCare.Product.Application.ApplicationServices
{
    public abstract class ChangeRequestService<T, TItem, TComment, TAttachment> : IChangeRequestService<T, TItem, TComment, TAttachment>
        where T : class
        where TItem : class
        where TComment : class
        where TAttachment : class
    {
        private readonly IChangeRequestBusinessService<T, TItem, TComment, TAttachment> _changeRequestBusinessService;

        protected ChangeRequestService(IChangeRequestBusinessService<T, TItem, TComment, TAttachment> changeRequestBusinessService)
        {
            _changeRequestBusinessService = changeRequestBusinessService;
        }

        public virtual async Task<long> SubmitAsync<TId>(T entity, TId id, string requestedBy)
        {
            return await _changeRequestBusinessService.SubmitAsync(entity, id, requestedBy);
        }

        public virtual async Task<bool> ApproveAsync<TKey>(TKey changeRequestId, string approvedBy)
        {
            var status = await _changeRequestBusinessService.ApproveAsync(changeRequestId, approvedBy);
            return status;
        }

        public virtual async Task<bool> RejectAsync<TKey>(TKey changeRequestId, string rejectedBy)
        {
            var status = await _changeRequestBusinessService.RejectAsync(changeRequestId, rejectedBy);
            return status;
        }

        public virtual async Task<List<T>> GetAllChangeRequestsAsync(int topCount = 0)
        {
            if (topCount == 0)
            {
                topCount =
                    Convert.ToInt32(
                        ConfigurationManager.AppSettings.Get("DefaultChangeRequestRecordCount"));
            }

            return await _changeRequestBusinessService.GetAllChangeRequestsAsync(topCount);
        }

        public virtual async Task<List<T>> GetChangeRequestsAsync(Expression<Func<T, bool>> whereCondition,
            int topCount = 0)
        {
            return await _changeRequestBusinessService.GetChangeRequestsAsync(whereCondition, topCount);
        }

        public virtual async Task<List<TItem>> GetChangeRequestItemStagingsAsync(
            Expression<Func<TItem, bool>> whereCondition, int topCount = 0)
        {
            return await _changeRequestBusinessService.GetChangeRequestItemStagingsAsync(whereCondition, topCount);
        }

        public virtual async Task<List<ChangeEntityFacet>> GetAllChangeTypes()
        {
            return await _changeRequestBusinessService.GetAllChangeTypes();
        }

        public virtual async Task<AssociationCount> GetAssociatedCount(List<ChangeRequestStaging> selectedChangeRequestStagings)
        {
            return await _changeRequestBusinessService.GetAssociatedCount(selectedChangeRequestStagings);
        }

        public virtual async Task<bool> AssignReviewer(AssignReviewerBusinessModel assignReviewerBusinessModel)
        {
            return await _changeRequestBusinessService.AssignReviewer(assignReviewerBusinessModel);
        }

        public virtual async Task<List<CommentsStagingModel>> GetRequestorComments(string changeRequestId,string reviewStatus)
        {
            return await _changeRequestBusinessService.GetRequestorComments(changeRequestId, reviewStatus);
        }
    }
}
