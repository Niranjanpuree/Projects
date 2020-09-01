using System.Threading.Tasks;
using AutoCare.Product.Vcdb.Model;
using AutoCare.Product.Infrastructure.Bus.EventBus;
using AutoCare.Product.Infrastructure.Serializer;
using AutoCare.Product.Application.BusinessServices.DocumentIndexer;
using AutoCare.Product.Application.RepositoryServices;
using AutoCare.Product.Application.BusinessServices.Event;

namespace AutoCare.Product.Application.BusinessServices.EventHandler
{
    public class BrakeABSChangeRequestReviewEventHandler: ChangeRequestReviewEventHandler<BrakeABS>, 
        IEventHandler<ApprovedEvent<BrakeABS>>,
        IEventHandler<RejectedEvent<BrakeABS>>,
        IEventHandler<DeletedEvent<BrakeABS>>
    {
        public BrakeABSChangeRequestReviewEventHandler(IVcdbUnitOfWork vcdbUnitOfWork,
            ITextSerializer serializer,
            IBrakeABSDataIndexer dataIndexer)
            : base(vcdbUnitOfWork, serializer, dataIndexer)
        {
        }

        public async Task HandleAsync(ApprovedEvent<BrakeABS> changeRequestApprovedEvent)
        {
            await RaiseApprovedEventAction(changeRequestApprovedEvent);
        }

        public async Task HandleAsync(RejectedEvent<BrakeABS> changeRequestRejectedEvent)
        {
            await RaiseRejectedEventAction(changeRequestRejectedEvent);
        }

        public async Task HandleAsync(DeletedEvent<BrakeABS> changeRequestDeletedEvent)
        {
            await RaiseDeletedEventAction(changeRequestDeletedEvent);
        }
    }
}
