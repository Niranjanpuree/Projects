using System.Collections.Generic;
using System.Threading.Tasks;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.BusinessServices.ChangeRequestReviewProcessors
{
    public interface IPreliminaryApproveChangeRequestProcessor
    {
        Task<bool> ProcessAsync(long changeRequestId, string approvedBy,
            CommentsStaging commentsStaging = null, List<AttachmentsStaging> attachments = null);

        Task<long> AddCommentsAsync(long changeRequestId, CommentsStaging commentsStaging, string approvedBy);

        Task SaveAttachmentsAsync(List<AttachmentsStaging> attachmentsStaging, ChangeRequestStaging changeRequestStaging);

        Task<ChangeRequestStaging> UpdateChangeRequestStatusAsync(long changeRequestId);
    }
}
