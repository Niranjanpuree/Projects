using AutoCare.Product.Application.BusinessServices.ChangeRequestReviewProcessors;
using AutoCare.Product.Application.RepositoryServices;
using AutoCare.Product.Infrastructure.Bus.EventBus;
using AutoCare.Product.Infrastructure.Serializer;
using AutoCare.Product.VcdbSearch.Indexing.ApplicationService;

namespace AutoCare.Product.Application.BusinessServices.Vcdb
{
    public class VcdbApproveChangeRequestProcessor : ApproveChangeRequestProcessor, IVcdbApproveChangeRequestProcessor
    {
        private const string VcdbContainerName = "vcdbattachments";

        public VcdbApproveChangeRequestProcessor(
            IVcdbChangeRequestCommentsBusinessService commentsStagingBusinessService,
            IVcdbChangeRequestAttachmentBusinessService attachmentStagingBusinessService,
            IVcdbChangeRequestItemBusinessService changeRequestItemStagingBusinessService,
            IAzureFileStorageRepositoryService azureFileStorageRepositoryService, ITextSerializer serializer,
            IEventBus eventLocalBus,
            IVcdbUnitOfWork unitofWork,
            IChangeRequestIndexingService changeRequestIndexingService)
            : base(
                commentsStagingBusinessService, attachmentStagingBusinessService,
                changeRequestItemStagingBusinessService, azureFileStorageRepositoryService, serializer, unitofWork,
                eventLocalBus, changeRequestIndexingService,
                VcdbContainerName)
        {
        }
    }
}
