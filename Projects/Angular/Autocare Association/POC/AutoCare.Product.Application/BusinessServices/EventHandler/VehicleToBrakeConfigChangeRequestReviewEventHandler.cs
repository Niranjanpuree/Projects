using System.Threading.Tasks;
using AutoCare.Product.Application.BusinessServices.DocumentIndexer;
using AutoCare.Product.Application.BusinessServices.Event;
using AutoCare.Product.Application.RepositoryServices;
using AutoCare.Product.Infrastructure.Bus.EventBus;
using AutoCare.Product.Infrastructure.Serializer;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.BusinessServices.EventHandler
{
    public class VehicleToBrakeConfigChangeRequestReviewEventHandler
        : ChangeRequestReviewEventHandler<VehicleToBrakeConfig>,
        IEventHandler<ApprovedEvent<VehicleToBrakeConfig>>,
        IEventHandler<RejectedEvent<VehicleToBrakeConfig>>,
        IEventHandler<DeletedEvent<VehicleToBrakeConfig>>
    {
        public VehicleToBrakeConfigChangeRequestReviewEventHandler(IVcdbUnitOfWork vcdbUnitOfWork,
            ITextSerializer serializer,
            IVehicleToBrakeConfigDataIndexer documentIndexer = null)
            : base(vcdbUnitOfWork, serializer, documentIndexer)
        {
        }

        public async Task HandleAsync(ApprovedEvent<VehicleToBrakeConfig> changeRequestApprovedEvent)
        {
            await RaiseApprovedEventAction(changeRequestApprovedEvent);
        }

        public async Task HandleAsync(RejectedEvent<VehicleToBrakeConfig> changeRequestRejectedEvent)
        {
            await RaiseRejectedEventAction(changeRequestRejectedEvent);
        }

        public async Task HandleAsync(DeletedEvent<VehicleToBrakeConfig> changeRequestDeletedEvent)
        {
            await RaiseDeletedEventAction(changeRequestDeletedEvent);
        }

        protected override async Task ApprovedReplaceChangeRequestAction(ApprovedEvent<VehicleToBrakeConfig> approvedEvent)
        {
            await base.ApprovedReplaceChangeRequestAction(approvedEvent);
            await ClearChangeRequestId<BrakeConfig>(approvedEvent.ChangeRequestId);
        }

        protected override async Task RejectedReplaceChangeRequestAction(RejectedEvent<VehicleToBrakeConfig> rejectedEvent)
        {
            await base.RejectedReplaceChangeRequestAction(rejectedEvent);
            await ClearChangeRequestId<BrakeConfig>(rejectedEvent.ChangeRequestId);
        }

        protected override async Task DeletedReplaceChangeRequestAction(DeletedEvent<VehicleToBrakeConfig> deletedEvent)
        {
            await base.DeletedReplaceChangeRequestAction(deletedEvent);
            await ClearChangeRequestId<BrakeConfig>(deletedEvent.ChangeRequestId);
        }
    }
}
