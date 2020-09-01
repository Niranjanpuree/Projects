using System.Threading.Tasks;
using AutoCare.Product.Application.BusinessServices.DocumentIndexer;
using AutoCare.Product.Vcdb.Model;
using AutoCare.Product.Infrastructure.Bus.EventBus;
using AutoCare.Product.Infrastructure.Serializer;
using AutoCare.Product.Application.RepositoryServices;
using AutoCare.Product.Application.BusinessServices.Event;

namespace AutoCare.Product.Application.BusinessServices.EventHandler
{
    public class VehicleTypeChangeRequestReviewEventHandler : ChangeRequestReviewEventHandler<VehicleType>,
        IEventHandler<ApprovedEvent<VehicleType>>,
        IEventHandler<RejectedEvent<VehicleType>>,
        IEventHandler<DeletedEvent<VehicleType>>
    {
        public VehicleTypeChangeRequestReviewEventHandler(IVcdbUnitOfWork vcdbUnitOfWork,
           ITextSerializer serializer, IVehicleTypeDataIndexer documentIndexer)
            : base(vcdbUnitOfWork, serializer, documentIndexer)
        {
                
        }

        public async Task HandleAsync(ApprovedEvent<VehicleType> changeRequestApprovedEvent)
        {
            await RaiseApprovedEventAction(changeRequestApprovedEvent);
        }

        public async Task HandleAsync(RejectedEvent<VehicleType> changeRequestRejectedEvent)
        {
            await RaiseRejectedEventAction(changeRequestRejectedEvent);
        }

        public async Task HandleAsync(DeletedEvent<VehicleType> changeRequestDeletedEvent)
        {
            await RaiseDeletedEventAction(changeRequestDeletedEvent);
        }
    }
}
