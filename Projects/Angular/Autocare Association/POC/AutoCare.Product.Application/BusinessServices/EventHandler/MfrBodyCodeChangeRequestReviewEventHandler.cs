using AutoCare.Product.Application.BusinessServices.DocumentIndexer;
using AutoCare.Product.Application.BusinessServices.Event;
using AutoCare.Product.Application.RepositoryServices;
using AutoCare.Product.Infrastructure.Bus.EventBus;
using AutoCare.Product.Infrastructure.Serializer;
using AutoCare.Product.Vcdb.Model;
using System.Threading.Tasks;

namespace AutoCare.Product.Application.BusinessServices.EventHandler
{
    public class MfrBodyCodeChangeRequestReviewEventHandler: ChangeRequestReviewEventHandler<MfrBodyCode>,
        IEventHandler<ApprovedEvent<MfrBodyCode>>,
        IEventHandler<RejectedEvent<MfrBodyCode>>,
        IEventHandler<DeletedEvent<MfrBodyCode>>
    {
        public MfrBodyCodeChangeRequestReviewEventHandler(IVcdbUnitOfWork vcdbUnitOfWork,
            ITextSerializer serializer,
            IMfrBodyCodeDataIndexer documentIndexer = null)
            : base(vcdbUnitOfWork, serializer, documentIndexer)
        {
        }
        public async Task HandleAsync(ApprovedEvent<MfrBodyCode> changeRequestApprovedEvent)
        {
            await RaiseApprovedEventAction(changeRequestApprovedEvent);
        }

        public async Task HandleAsync(RejectedEvent<MfrBodyCode> changeRequestRejectedEvent)
        {
            await RaiseRejectedEventAction(changeRequestRejectedEvent);
        }

        public async Task HandleAsync(DeletedEvent<MfrBodyCode> changeRequestDeletedEvent)
        {
            await RaiseDeletedEventAction(changeRequestDeletedEvent);
        }
    }
}
