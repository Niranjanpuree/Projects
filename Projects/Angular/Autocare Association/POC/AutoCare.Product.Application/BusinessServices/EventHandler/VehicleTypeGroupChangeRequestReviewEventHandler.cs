using System.Threading.Tasks;
using AutoCare.Product.Application.BusinessServices.DocumentIndexer;
using AutoCare.Product.Vcdb.Model;
using AutoCare.Product.Infrastructure.Bus.EventBus;
using AutoCare.Product.Infrastructure.Serializer;
using AutoCare.Product.Application.RepositoryServices;
using AutoCare.Product.Application.BusinessServices.Event;

namespace AutoCare.Product.Application.BusinessServices.EventHandler
{
    public class VehicleTypeGroupChangeRequestReviewEventHandler : ChangeRequestReviewEventHandler<VehicleTypeGroup>,
        IEventHandler<ApprovedEvent<VehicleTypeGroup>>,
        IEventHandler<RejectedEvent<VehicleTypeGroup>>,
        IEventHandler<DeletedEvent<VehicleTypeGroup>>
    {
        public VehicleTypeGroupChangeRequestReviewEventHandler(IVcdbUnitOfWork vcdbUnitOfWork,
           ITextSerializer serializer, IVehicleTypeGroupDataIndexer documentIndexer)
            : base(vcdbUnitOfWork, serializer, documentIndexer)
        {
            
        }

        public async Task HandleAsync(ApprovedEvent<VehicleTypeGroup> changeRequestApprovedEvent)
        {
            await RaiseApprovedEventAction(changeRequestApprovedEvent);
        }

        public async Task HandleAsync(RejectedEvent<VehicleTypeGroup> changeRequestRejectedEvent)
        {
            await RaiseRejectedEventAction(changeRequestRejectedEvent);
        }

        public async Task HandleAsync(DeletedEvent<VehicleTypeGroup> changeRequestDeletedEvent)
        {
            await RaiseDeletedEventAction(changeRequestDeletedEvent);
        }
    }
}
