using System.Threading.Tasks;

namespace AutoCare.Product.Infrastructure.Bus.EventBus
{
    public interface IEventHandler<in T>
        where T : class, IEvent
    {
        Task HandleAsync(T @event);
    }
}
