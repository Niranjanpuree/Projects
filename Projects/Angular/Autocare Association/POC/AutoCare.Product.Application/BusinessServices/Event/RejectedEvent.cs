using AutoCare.Product.Infrastructure.Bus.EventBus;

namespace AutoCare.Product.Application.BusinessServices.Event
{
    public class RejectedEvent<TEntity> : ChangeRequestReviewEvent, IEvent
        where TEntity : class
    {
    }
}
