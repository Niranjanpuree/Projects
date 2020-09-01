using AutoCare.Product.Infrastructure.Bus.EventBus;

namespace AutoCare.Product.Application.BusinessServices.Event
{
    public class DeletedEvent<TEntity> : ChangeRequestReviewEvent, IEvent
        where TEntity : class
    {
    }
}
