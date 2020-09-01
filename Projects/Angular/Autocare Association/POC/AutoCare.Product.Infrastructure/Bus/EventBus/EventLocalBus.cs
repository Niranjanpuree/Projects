using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoCare.Product.Infrastructure.Bus.EventBus
{
    public class EventLocalBus : IEventBus
    {
        private readonly IDictionary<Type, List<object>> _eventHandlers = null;
        public EventLocalBus()
        {
            _eventHandlers = new Dictionary<Type, List<object>>();
        }

        public Task RegisterAsync<T, THandler>(THandler eventHandler)
            where T : class, IEvent
            where THandler : class, IEventHandler<T>
        {
            return Task.Run(() =>
            {
                List<object> handlers;
                if (!_eventHandlers.TryGetValue(typeof(T), out handlers))
                {
                    handlers = new List<object>();
                    _eventHandlers.Add(typeof(T), handlers);
                }

                handlers.Add(eventHandler);

            });
        }

        public Task RegisterAsync<T, THandler>(Func<THandler> eventHandler)
            where T : class, IEvent
            where THandler : class, IEventHandler<T>
        {
            return Task.Run(() =>
            {
                List<object> handlers;
                if (!_eventHandlers.TryGetValue(typeof(T), out handlers))
                {
                    handlers = new List<object>();
                    _eventHandlers.Add(typeof(T), handlers);
                }

                handlers.Add(eventHandler);

            });
        }

        public async Task SendAsync<T>(T @event)
            where T : class, IEvent
        {
            await Send(@event);
        }

        private Task Send<T>(T @event)
            where T : class, IEvent
        {
            return Task.Run(async () =>
            {
                List<object> handlers;
                if (!_eventHandlers.TryGetValue(@event.GetType(), out handlers))
                {
                    return;
                }

                foreach (var handler in handlers)
                {
                    object handlerInstance = null;
                    if (handler.GetType().IsGenericType)
                    {
                        if (handler.GetType().GetGenericTypeDefinition() == typeof (Func<>))
                        {
                            var handlerFunc = handler as Delegate;
                            handlerInstance = handlerFunc?.DynamicInvoke();
                        }
                    }
                    else
                    {
                        handlerInstance = handler;
                    }

                    if (handlerInstance == null)
                    {
                        throw new ArgumentException("No event handler found");
                    }

                    var handlerMethod = handlerInstance.GetType().GetMethod("HandleAsync", new Type[] { @event.GetType() });

                    var task = handlerMethod?.Invoke(handlerInstance, new object[] { @event }) as Task;
                    if (task != null && task.GetType().BaseType == typeof(Task))
                    {
                        await task;
                    }
                }

            });
        }
    }
}
