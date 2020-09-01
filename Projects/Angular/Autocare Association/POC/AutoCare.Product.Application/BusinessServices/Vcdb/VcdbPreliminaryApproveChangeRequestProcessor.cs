using AutoCare.Product.Application.BusinessServices.ChangeRequestReviewProcessors;
using AutoCare.Product.Application.RepositoryServices;
using AutoCare.Product.Infrastructure.Bus.EventBus;
using AutoCare.Product.Infrastructure.Serializer;
using AutoCare.Product.Vcdb.Model;
using AutoCare.Product.VcdbSearch.Indexing.ApplicationService;

namespace AutoCare.Product.Application.BusinessServices.Vcdb
{
    public class VcdbPreliminaryApproveChangeRequestProcessor : PreliminaryApproveChangeRequestProcessor, IVcdbPreliminaryApproveChangeRequestProcessor
    {
        private const string VcdbContainerName = "vcdbattachments";

        public VcdbPreliminaryApproveChangeRequestProcessor(
            IVcdbChangeRequestCommentsBusinessService commentsStagingBusinessService,
            IVcdbChangeRequestAttachmentBusinessService attachmentStagingBusinessService,
            IVcdbSqlServerEfRepositoryService<ChangeRequestStaging> changeRequestStagingRepositoryService,
            IVcdbChangeRequestItemBusinessService changeRequestItemStagingBusinessService,
            IVcdbSqlServerEfRepositoryService<ChangeRequestAssignmentStaging> assignmentStagingRepositoryService,
            IVcdbSqlServerEfRepositoryService<ChangeRequestStore> changeRequestStoreRepositoryService,
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
