using System.Threading.Tasks;
using AutoCare.Product.Application.BusinessServices.DocumentIndexer;
using AutoCare.Product.Vcdb.Model;
using AutoCare.Product.Infrastructure.Bus.EventBus;
using AutoCare.Product.Infrastructure.Serializer;
using AutoCare.Product.Application.RepositoryServices;
using AutoCare.Product.Application.BusinessServices.Event;

namespace AutoCare.Product.Application.BusinessServices.EventHandler
{
    public class YearChangeRequestReviewEventHandler : ChangeRequestReviewEventHandler<Year>,
        IEventHandler<ApprovedEvent<Year>>,
        IEventHandler<RejectedEvent<Year>>,
        IEventHandler<DeletedEvent<Year>>
    {
        public YearChangeRequestReviewEventHandler(IVcdbUnitOfWork vcdbUnitOfWork,
           ITextSerializer serializer, IYearDataIndexer documentIndexer)
            : base(vcdbUnitOfWork, serializer, documentIndexer)
        {
        }

        public async Task HandleAsync(ApprovedEvent<Year> changeRequestApprovedEvent)
        {
            await RaiseApprovedEventAction(changeRequestApprovedEvent);
        }

        public async Task HandleAsync(RejectedEvent<Year> changeRequestRejectedEvent)
        {
            await RaiseRejectedEventAction(changeRequestRejectedEvent);
        }

        public async Task HandleAsync(DeletedEvent<Year> changeRequestDeletedEvent)
        {
            await RaiseDeletedEventAction(changeRequestDeletedEvent);
        }
    }
}
