using AutoCare.Product.Application.BusinessServices.DocumentIndexer;
using AutoCare.Product.Application.BusinessServices.Event;
using AutoCare.Product.Application.RepositoryServices;
using AutoCare.Product.Infrastructure.Bus.EventBus;
using AutoCare.Product.Infrastructure.Serializer;
using AutoCare.Product.Vcdb.Model;
using System.Threading.Tasks;

namespace AutoCare.Product.Application.BusinessServices.EventHandler
{
    public class DriveTypeChangeRequestReviewEventHandler: ChangeRequestReviewEventHandler<DriveType>,
        IEventHandler<ApprovedEvent<DriveType>>,
        IEventHandler<RejectedEvent<DriveType>>,
        IEventHandler<DeletedEvent<DriveType>>
    {
        public DriveTypeChangeRequestReviewEventHandler(IVcdbUnitOfWork vcdbUnitOfWork,
            ITextSerializer serializer,
            IDriveTypeDataIndexer documentIndexer = null)
            : base(vcdbUnitOfWork, serializer, documentIndexer)
        {
        }
        public async Task HandleAsync(ApprovedEvent<DriveType> changeRequestApprovedEvent)
        {
            await RaiseApprovedEventAction(changeRequestApprovedEvent);
        }

        public async Task HandleAsync(RejectedEvent<DriveType> changeRequestRejectedEvent)
        {
            await RaiseRejectedEventAction(changeRequestRejectedEvent);
        }

        public async Task HandleAsync(DeletedEvent<DriveType> changeRequestDeletedEvent)
        {
            await RaiseDeletedEventAction(changeRequestDeletedEvent);
        }
    }
}
