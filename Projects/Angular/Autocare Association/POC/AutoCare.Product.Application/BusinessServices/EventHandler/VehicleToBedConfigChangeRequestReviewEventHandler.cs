using System.Threading.Tasks;
using AutoCare.Product.Application.BusinessServices.DocumentIndexer;
using AutoCare.Product.Application.BusinessServices.Event;
using AutoCare.Product.Application.RepositoryServices;
using AutoCare.Product.Infrastructure.Bus.EventBus;
using AutoCare.Product.Infrastructure.Serializer;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.BusinessServices.EventHandler
{
    public class VehicleToBedConfigChangeRequestReviewEventHandler
        : ChangeRequestReviewEventHandler<VehicleToBedConfig>,
        IEventHandler<ApprovedEvent<VehicleToBedConfig>>,
        IEventHandler<RejectedEvent<VehicleToBedConfig>>,
        IEventHandler<DeletedEvent<VehicleToBedConfig>>
    {
        public VehicleToBedConfigChangeRequestReviewEventHandler(IVcdbUnitOfWork vcdbUnitOfWork,
            ITextSerializer serializer,
            IVehicleToBedConfigDataIndexer documentIndexer = null)
            : base(vcdbUnitOfWork, serializer, documentIndexer)
        {
        }

        public async Task HandleAsync(ApprovedEvent<VehicleToBedConfig> changeRequestApprovedEvent)
        {
            await RaiseApprovedEventAction(changeRequestApprovedEvent);
        }

        public async Task HandleAsync(RejectedEvent<VehicleToBedConfig> changeRequestRejectedEvent)
        {
            await RaiseRejectedEventAction(changeRequestRejectedEvent);
        }

        public async Task HandleAsync(DeletedEvent<VehicleToBedConfig> changeRequestDeletedEvent)
        {
            await RaiseDeletedEventAction(changeRequestDeletedEvent);
        }

        protected override async Task ApprovedReplaceChangeRequestAction(ApprovedEvent<VehicleToBedConfig> approvedEvent)
        {
            await base.ApprovedReplaceChangeRequestAction(approvedEvent);
            await ClearChangeRequestId<BedConfig>(approvedEvent.ChangeRequestId);
        }

        protected override async Task RejectedReplaceChangeRequestAction(RejectedEvent<VehicleToBedConfig> rejectedEvent)
        {
            await base.RejectedReplaceChangeRequestAction(rejectedEvent);
            await ClearChangeRequestId<BedConfig>(rejectedEvent.ChangeRequestId);
        }

        protected override async Task DeletedReplaceChangeRequestAction(DeletedEvent<VehicleToBedConfig> deletedEvent)
        {
            await base.DeletedReplaceChangeRequestAction(deletedEvent);
            await ClearChangeRequestId<BedConfig>(deletedEvent.ChangeRequestId);
        }
    }
}
