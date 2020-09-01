using AutoCare.Product.Infrastructure.Bus.EventBus;

namespace AutoCare.Product.Application.BusinessServices.Event
{
    public class ApprovedEvent<TEntity> : ChangeRequestReviewEvent, IEvent
        where TEntity : class
    {
    }
}
