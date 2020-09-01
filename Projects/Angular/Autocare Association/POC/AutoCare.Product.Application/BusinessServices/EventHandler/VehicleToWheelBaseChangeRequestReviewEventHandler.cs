using System.Threading.Tasks;
using AutoCare.Product.Application.BusinessServices.DocumentIndexer;
using AutoCare.Product.Application.BusinessServices.Event;
using AutoCare.Product.Application.RepositoryServices;
using AutoCare.Product.Infrastructure.Bus.EventBus;
using AutoCare.Product.Infrastructure.Serializer;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.BusinessServices.EventHandler
{
    public class VehicleToWheelBaseChangeRequestReviewEventHandler
        : ChangeRequestReviewEventHandler<VehicleToWheelBase>,
        IEventHandler<ApprovedEvent<VehicleToWheelBase>>,
        IEventHandler<RejectedEvent<VehicleToWheelBase>>,
        IEventHandler<DeletedEvent<VehicleToWheelBase>>
    {
        public VehicleToWheelBaseChangeRequestReviewEventHandler(IVcdbUnitOfWork vcdbUnitOfWork,
            ITextSerializer serializer,
            IVehicleToWheelBaseDataIndexer documentIndexer = null)
            : base(vcdbUnitOfWork, serializer, documentIndexer)
        {
        }

        public async Task HandleAsync(ApprovedEvent<VehicleToWheelBase> changeRequestApprovedEvent)
        {
            await RaiseApprovedEventAction(changeRequestApprovedEvent);
        }

        public async Task HandleAsync(RejectedEvent<VehicleToWheelBase> changeRequestRejectedEvent)
        {
            await RaiseRejectedEventAction(changeRequestRejectedEvent);
        }

        public async Task HandleAsync(DeletedEvent<VehicleToWheelBase> changeRequestDeletedEvent)
        {
            await RaiseDeletedEventAction(changeRequestDeletedEvent);
        }

        protected override async Task ApprovedReplaceChangeRequestAction(ApprovedEvent<VehicleToWheelBase> approvedEvent)
        {
            await base.ApprovedReplaceChangeRequestAction(approvedEvent);
            await ClearChangeRequestId<WheelBase>(approvedEvent.ChangeRequestId);
        }

        protected override async Task RejectedReplaceChangeRequestAction(RejectedEvent<VehicleToWheelBase> rejectedEvent)
        {
            await base.RejectedReplaceChangeRequestAction(rejectedEvent);
            await ClearChangeRequestId<WheelBase>(rejectedEvent.ChangeRequestId);
        }

        protected override async Task DeletedReplaceChangeRequestAction(DeletedEvent<VehicleToWheelBase> deletedEvent)
        {
            await base.DeletedReplaceChangeRequestAction(deletedEvent);
            await ClearChangeRequestId<WheelBase>(deletedEvent.ChangeRequestId);
        }
    }
}
