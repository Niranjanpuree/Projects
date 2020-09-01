using System.Collections.Generic;
using System.Threading.Tasks;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.BusinessServices.ChangeRequestReviewProcessors
{
    public interface IApproveChangeRequestProcessor
    {
        Task<bool> ProcessAsync(long changeRequestId, string approvedBy,
            CommentsStaging commentsStaging = null, List<AttachmentsStaging> attachments = null);

        Task<long> AddCommentsAsync(long changeRequestId, CommentsStaging commentsStaging, string approvedBy);

        Task SaveAttachmentsAsync(List<AttachmentsStaging> attachmentsStaging, ChangeRequestStaging changeRequestStaging);

        Task<ChangeRequestStaging> UpdateChangeRequestStatusAsync(long changeRequestId);

        Task<ChangeRequestStore> AddToChangeRequestStoreAsync(long changeRequestId, ChangeRequestStatus status, string decisionBy);

        Task<ChangeRequestStaging> RemoveFromChangeRequestStagingAsync(long changeRequestId);

        Task<bool> UpdateToTransactionalTableAsync(ChangeRequestStore changeRequestStore, long changeRequestId);

        Task UpdateChangeRequestStatusToChangeRequestIndexAsync(long changeRequestId, ChangeRequestStatus changeRequestStatus, long reviewCommentId);
    }
}
