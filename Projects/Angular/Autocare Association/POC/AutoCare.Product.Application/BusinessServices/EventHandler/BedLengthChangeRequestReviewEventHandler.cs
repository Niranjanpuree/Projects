using System.Threading.Tasks;
using AutoCare.Product.Vcdb.Model;
using AutoCare.Product.Infrastructure.Bus.EventBus;
using AutoCare.Product.Infrastructure.Serializer;
using AutoCare.Product.Application.BusinessServices.DocumentIndexer;
using AutoCare.Product.Application.RepositoryServices;
using AutoCare.Product.Application.BusinessServices.Event;

namespace AutoCare.Product.Application.BusinessServices.EventHandler
{
    public class BedLengthChangeRequestReviewEventHandler : ChangeRequestReviewEventHandler<BedLength>,
        IEventHandler<ApprovedEvent<BedLength>>,
        IEventHandler<RejectedEvent<BedLength>>,
        IEventHandler<DeletedEvent<BedLength>>
    {
        public BedLengthChangeRequestReviewEventHandler(IVcdbUnitOfWork vcdbUnitOfWork,
            ITextSerializer serializer,
            IBedLengthDataIndexer dataIndexer)
            : base(vcdbUnitOfWork, serializer, dataIndexer)
        {
        }

        public async Task HandleAsync(ApprovedEvent<BedLength> changeRequestApprovedEvent)
        {
            await RaiseApprovedEventAction(changeRequestApprovedEvent);
        }

        public async Task HandleAsync(RejectedEvent<BedLength> changeRequestRejectedEvent)
        {
            await RaiseRejectedEventAction(changeRequestRejectedEvent);
        }

        public async Task HandleAsync(DeletedEvent<BedLength> changeRequestDeletedEvent)
        {
            await RaiseDeletedEventAction(changeRequestDeletedEvent);
        }
    }
}
