using System.Threading.Tasks;
using AutoCare.Product.Application.BusinessServices.DocumentIndexer;
using AutoCare.Product.Vcdb.Model;
using AutoCare.Product.Infrastructure.Bus.EventBus;
using AutoCare.Product.Infrastructure.Serializer;
using AutoCare.Product.Application.RepositoryServices;
using AutoCare.Product.Application.BusinessServices.Event;

namespace AutoCare.Product.Application.BusinessServices.EventHandler
{
    public class MakeChangeRequestReviewEventHandler : ChangeRequestReviewEventHandler<Make>, 
        IEventHandler<ApprovedEvent<Make>>,
        IEventHandler<RejectedEvent<Make>>,
        IEventHandler<DeletedEvent<Make>>
    {
        public MakeChangeRequestReviewEventHandler(IVcdbUnitOfWork vcdbUnitOfWork,
           ITextSerializer serializer, IMakeDataIndexer documentIndexer)
            : base(vcdbUnitOfWork, serializer, documentIndexer)
        {
        }

        public async Task HandleAsync(ApprovedEvent<Make> changeRequestApprovedEvent)
        {
            await RaiseApprovedEventAction(changeRequestApprovedEvent);
        }

        public async Task HandleAsync(RejectedEvent<Make> changeRequestRejectedEvent)
        {
            await RaiseRejectedEventAction(changeRequestRejectedEvent);
        }

        public async Task HandleAsync(DeletedEvent<Make> changeRequestDeletedEvent)
        {
            await RaiseDeletedEventAction(changeRequestDeletedEvent);
        }
    }
}





