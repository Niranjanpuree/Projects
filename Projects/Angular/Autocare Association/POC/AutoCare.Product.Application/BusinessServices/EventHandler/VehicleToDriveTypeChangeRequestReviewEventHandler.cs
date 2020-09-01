using System.Threading.Tasks;
using AutoCare.Product.Application.BusinessServices.DocumentIndexer;
using AutoCare.Product.Application.BusinessServices.Event;
using AutoCare.Product.Application.RepositoryServices;
using AutoCare.Product.Infrastructure.Bus.EventBus;
using AutoCare.Product.Infrastructure.Serializer;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.BusinessServices.EventHandler
{
    public class VehicleToDriveTypeChangeRequestReviewEventHandler
        : ChangeRequestReviewEventHandler<VehicleToDriveType>,
        IEventHandler<ApprovedEvent<VehicleToDriveType>>,
        IEventHandler<RejectedEvent<VehicleToDriveType>>,
        IEventHandler<DeletedEvent<VehicleToDriveType>>
    {
        public VehicleToDriveTypeChangeRequestReviewEventHandler(IVcdbUnitOfWork vcdbUnitOfWork,
            ITextSerializer serializer,
            IVehicleToDriveTypeDataIndexer documentIndexer = null)
            : base(vcdbUnitOfWork, serializer, documentIndexer)
        {
        }

        public async Task HandleAsync(ApprovedEvent<VehicleToDriveType> changeRequestApprovedEvent)
        {
            await RaiseApprovedEventAction(changeRequestApprovedEvent);
        }

        public async Task HandleAsync(RejectedEvent<VehicleToDriveType> changeRequestRejectedEvent)
        {
            await RaiseRejectedEventAction(changeRequestRejectedEvent);
        }

        public async Task HandleAsync(DeletedEvent<VehicleToDriveType> changeRequestDeletedEvent)
        {
            await RaiseDeletedEventAction(changeRequestDeletedEvent);
        }

        protected override async Task ApprovedReplaceChangeRequestAction(ApprovedEvent<VehicleToDriveType> approvedEvent)
        {
            await base.ApprovedReplaceChangeRequestAction(approvedEvent);
            await ClearChangeRequestId<DriveType>(approvedEvent.ChangeRequestId);
        }

        protected override async Task RejectedReplaceChangeRequestAction(RejectedEvent<VehicleToDriveType> rejectedEvent)
        {
            await base.RejectedReplaceChangeRequestAction(rejectedEvent);
            await ClearChangeRequestId<DriveType>(rejectedEvent.ChangeRequestId);
        }

        protected override async Task DeletedReplaceChangeRequestAction(DeletedEvent<VehicleToDriveType> deletedEvent)
        {
            await base.DeletedReplaceChangeRequestAction(deletedEvent);
            await ClearChangeRequestId<DriveType>(deletedEvent.ChangeRequestId);
        }
    }
}
