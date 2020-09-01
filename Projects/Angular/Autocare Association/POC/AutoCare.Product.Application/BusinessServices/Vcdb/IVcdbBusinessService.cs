using System.Collections.Generic;
using System.Threading.Tasks;
using AutoCare.Product.Application.Models.BusinessModels;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.BusinessServices.Vcdb
{
    public interface IVcdbBusinessService<T> : IBusinessService<T>
        where T : class
    {
        Task<long> SubmitAddChangeRequestAsync(T entity, string requestedBy,
            List<ChangeRequestItemStaging> changeRequestItemStagings = null,
            CommentsStagingModel comment = null, List<AttachmentsModel> attachments = null, string changeContent = null);
        Task<long> SubmitUpdateChangeRequestAsync<TId>(T entity, TId id, string requestedBy,
            ChangeType changeType = ChangeType.None,
            List<ChangeRequestItemStaging> changeRequestItemStagings = null,
            CommentsStagingModel comment = null, List<AttachmentsModel> attachments = null, string changeContent = null);
        Task<long> SubmitDeleteChangeRequestAsync<TId>(T entity, TId id, string requestedBy,
            List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null, List<AttachmentsModel> attachments = null, string changeContent = null);
        Task<long> SubmitReplaceChangeRequestAsync<TId>(T entity, TId id, string requestedBy,
            List<ChangeRequestItemStaging> changeRequestItemStagings = null, CommentsStagingModel comment = null, List<AttachmentsModel> attachments = null, string changeContent = null);

        //Task<bool> ChangeRequestExist<TId>(T entity, TId id);
        //Task<List<T>> GetPendingAddChangeRequests(Expression<Func<T, bool>> whereCondition = null, int topCount = 0);
        //Task<ChangeRequestStagingModel<T>> GetChangeRequestStaging<TId>(TId changeRequestId);
        //Task<bool> SubmitChangeRequestReviewAsync<TId>(TId changeRequestId, ChangeRequestReviewModel review);
        //Task<List<ChangeEntityFacet>> GetAllChangeTypes();

        Task<AssociationCount> GetAssociatedCount(List<ChangeRequestStaging> selectedChangeRequestStagings);
    }
}
