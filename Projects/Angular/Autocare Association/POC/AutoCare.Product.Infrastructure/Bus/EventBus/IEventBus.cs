using System;
using System.Threading.Tasks;

namespace AutoCare.Product.Infrastructure.Bus.EventBus
{
    public interface IEventBus
    {
        Task RegisterAsync<T, THandler>(Func<THandler> eventHandler)
           where T : class, IEvent
           where THandler : class, IEventHandler<T>;
        Task SendAsync<T>(T @event) where T : class, IEvent;
    }
}
