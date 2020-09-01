using System.Threading.Tasks;
using AutoCare.Product.Application.BusinessServices.DocumentIndexer;
using AutoCare.Product.Application.BusinessServices.Event;
using AutoCare.Product.Application.RepositoryServices;
using AutoCare.Product.Infrastructure.Bus.EventBus;
using AutoCare.Product.Infrastructure.Serializer;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.BusinessServices.EventHandler
{
    public class VehicleToMfrBodyCodeChangeRequestReviewEventHandler
        : ChangeRequestReviewEventHandler<VehicleToMfrBodyCode>,
        IEventHandler<ApprovedEvent<VehicleToMfrBodyCode>>,
        IEventHandler<RejectedEvent<VehicleToMfrBodyCode>>,
        IEventHandler<DeletedEvent<VehicleToMfrBodyCode>>
    {
        public VehicleToMfrBodyCodeChangeRequestReviewEventHandler(IVcdbUnitOfWork vcdbUnitOfWork,
            ITextSerializer serializer,
            IVehicleToMfrBodyCodeDataIndexer documentIndexer = null)
            : base(vcdbUnitOfWork, serializer, documentIndexer)
        {
        }

        public async Task HandleAsync(ApprovedEvent<VehicleToMfrBodyCode> changeRequestApprovedEvent)
        {
            await RaiseApprovedEventAction(changeRequestApprovedEvent);
        }

        public async Task HandleAsync(RejectedEvent<VehicleToMfrBodyCode> changeRequestRejectedEvent)
        {
            await RaiseRejectedEventAction(changeRequestRejectedEvent);
        }

        public async Task HandleAsync(DeletedEvent<VehicleToMfrBodyCode> changeRequestDeletedEvent)
        {
            await RaiseDeletedEventAction(changeRequestDeletedEvent);
        }

        protected override async Task ApprovedReplaceChangeRequestAction(ApprovedEvent<VehicleToMfrBodyCode> approvedEvent)
        {
            await base.ApprovedReplaceChangeRequestAction(approvedEvent);
            await ClearChangeRequestId<MfrBodyCode>(approvedEvent.ChangeRequestId);
        }

        protected override async Task RejectedReplaceChangeRequestAction(RejectedEvent<VehicleToMfrBodyCode> rejectedEvent)
        {
            await base.RejectedReplaceChangeRequestAction(rejectedEvent);
            await ClearChangeRequestId<MfrBodyCode>(rejectedEvent.ChangeRequestId);
        }

        protected override async Task DeletedReplaceChangeRequestAction(DeletedEvent<VehicleToMfrBodyCode> deletedEvent)
        {
            await base.DeletedReplaceChangeRequestAction(deletedEvent);
            await ClearChangeRequestId<MfrBodyCode>(deletedEvent.ChangeRequestId);
        }
    }
}
