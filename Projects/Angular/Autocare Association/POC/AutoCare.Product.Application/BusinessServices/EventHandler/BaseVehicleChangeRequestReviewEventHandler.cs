using System.Threading.Tasks;
using AutoCare.Product.Application.BusinessServices.DocumentIndexer;
using AutoCare.Product.Application.BusinessServices.Event;
using AutoCare.Product.Application.RepositoryServices;
using AutoCare.Product.Infrastructure.Bus.EventBus;
using AutoCare.Product.Infrastructure.Serializer;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.BusinessServices.EventHandler
{
    public class BaseVehicleChangeRequestReviewEventHandler
        : ChangeRequestReviewEventHandler<BaseVehicle>, 
        IEventHandler<ApprovedEvent<BaseVehicle>>,
        IEventHandler<RejectedEvent<BaseVehicle>>,
        IEventHandler<DeletedEvent<BaseVehicle>>
    {
        //public BaseVehicleChangeRequestReviewEventHandler(IVcdbUnitOfWork vcdbUnitOfWork,
        //    ITextSerializer serializer,
        //    IBaseVehicleDataIndexer documentIndexer = null)
        //    : base(vcdbUnitOfWork, serializer, documentIndexer)
        //{
        //}

        public BaseVehicleChangeRequestReviewEventHandler(IVcdbUnitOfWork vcdbUnitOfWork,
            ITextSerializer serializer,
            IBaseVehicleDataIndexer documentIndexer = null)
            : base(vcdbUnitOfWork, serializer, documentIndexer)
        {
        }

        public async Task HandleAsync(ApprovedEvent<BaseVehicle> changeRequestApprovedEvent)
        {
            await RaiseApprovedEventAction(changeRequestApprovedEvent);
        }

        public async Task HandleAsync(RejectedEvent<BaseVehicle> changeRequestRejectedEvent)
        {
            await RaiseRejectedEventAction(changeRequestRejectedEvent);
        }

        public async Task HandleAsync(DeletedEvent<BaseVehicle> changeRequestDeletedEvent)
        {
            await RaiseDeletedEventAction(changeRequestDeletedEvent);
        }

        //protected override async Task ApprovedAddChangeRequestAction(ApprovedEvent<BaseVehicle> approvedEvent)
        //{
        //    await base.ApprovedAddChangeRequestAction(approvedEvent);
        //    await ClearChangeRequestId<Vehicle>(approvedEvent.ChangeRequestId);
        //}

        //private async Task ClearChangeRequestId(long changeRequestId)
        //{
        //    await ClearChangeRequestId<BaseVehicle>(changeRequestId);
        //    await ClearChangeRequestId<Vehicle>(changeRequestId);
        //}
    }
}
