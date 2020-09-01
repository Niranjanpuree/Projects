using System.Threading.Tasks;
using AutoCare.Product.Application.BusinessServices.DocumentIndexer;
using AutoCare.Product.Vcdb.Model;
using AutoCare.Product.Infrastructure.Bus.EventBus;
using AutoCare.Product.Infrastructure.Serializer;
using AutoCare.Product.Application.RepositoryServices;
using AutoCare.Product.Application.BusinessServices.Event;

namespace AutoCare.Product.Application.BusinessServices.EventHandler
{
    public class RegionChangeRequestReviewEventHandler : ChangeRequestReviewEventHandler<Region>,
        IEventHandler<ApprovedEvent<Region>>,
        IEventHandler<RejectedEvent<Region>>,
        IEventHandler<DeletedEvent<Region>>
    {
        public RegionChangeRequestReviewEventHandler(IVcdbUnitOfWork vcdbUnitOfWork,
           ITextSerializer serializer, IRegionDataIndexer documentIndexer)
            : base(vcdbUnitOfWork, serializer, documentIndexer)
        {
        }

        public async Task HandleAsync(ApprovedEvent<Region> changeRequestApprovedEvent)
        {
            await RaiseApprovedEventAction(changeRequestApprovedEvent);
        }

        public async Task HandleAsync(RejectedEvent<Region> changeRequestRejectedEvent)
        {
            await RaiseRejectedEventAction(changeRequestRejectedEvent);
        }

        public async Task HandleAsync(DeletedEvent<Region> changeRequestDeletedEvent)
        {
            await RaiseDeletedEventAction(changeRequestDeletedEvent);
        }
    }
}
