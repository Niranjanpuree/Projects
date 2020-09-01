using System.Collections.Generic;
using System.Threading.Tasks;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.BusinessServices.ChangeRequestReviewProcessors
{
    public interface IRejectChangeRequestProcessor
    {
        Task<bool> ProcessAsync(long changeRequestId, string rejectedBy,
            CommentsStaging commentsStaging = null, List<AttachmentsStaging> attachments = null);

        Task<long> AddCommentsAsync(long changeRequestId, CommentsStaging commentsStaging, string rejectedBy);

        Task SaveAttachmentsAsync(List<AttachmentsStaging> attachmentsStaging, ChangeRequestStaging changeRequestStaging);

        Task<bool> UpdateToTransactionalTableAsync(ChangeRequestStore changeRequestStore, long changeRequestId);
    }
}
