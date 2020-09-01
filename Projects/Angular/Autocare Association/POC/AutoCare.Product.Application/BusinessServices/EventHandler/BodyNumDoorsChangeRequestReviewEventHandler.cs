using System.Threading.Tasks;
using AutoCare.Product.Application.BusinessServices.DocumentIndexer;
using AutoCare.Product.Vcdb.Model;
using AutoCare.Product.Infrastructure.Bus.EventBus;
using AutoCare.Product.Infrastructure.Serializer;
using AutoCare.Product.Application.RepositoryServices;
using AutoCare.Product.Application.BusinessServices.Event;

namespace AutoCare.Product.Application.BusinessServices.EventHandler
{
    public class BodyNumDoorsChangeRequestReviewEventHandler : ChangeRequestReviewEventHandler<BodyNumDoors>,
        IEventHandler<ApprovedEvent<BodyNumDoors>>,
        IEventHandler<RejectedEvent<BodyNumDoors>>,
        IEventHandler<DeletedEvent<BodyNumDoors>>
    {
        public BodyNumDoorsChangeRequestReviewEventHandler(IVcdbUnitOfWork vcdbUnitOfWork,
            ITextSerializer serializer, IBodyNumDoorsDataIndexer documentIndexer)
            : base(vcdbUnitOfWork, serializer, documentIndexer)
        {
        }
        public async Task HandleAsync(ApprovedEvent<BodyNumDoors> changeRequestApprovedEvent)
        {
            await RaiseApprovedEventAction(changeRequestApprovedEvent);
        }

        public async Task HandleAsync(RejectedEvent<BodyNumDoors> changeRequestRejectedEvent)
        {
            await RaiseRejectedEventAction(changeRequestRejectedEvent);
        }

        public async Task HandleAsync(DeletedEvent<BodyNumDoors> changeRequestDeletedEvent)
        {
            await RaiseDeletedEventAction(changeRequestDeletedEvent);
        }
    }
}
