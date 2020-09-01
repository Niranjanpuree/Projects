using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoCare.Product.Application.Infrastructure.Bus;
using AutoCare.Product.Application.Infrastructure.Bus.Event;

namespace AutoCare.Product.Application.Infrastructure.Domain
{
    public interface IEventSourcedEntity : IEntity
    {
        void RegisterEventHandler<TEvent>(Action<TEvent> eventHandler)
            where TEvent : class, IEvent, IMessage;

        void AddEvent<TEvent>(TEvent @event)
            where TEvent : class, IEvent, IMessage;
    }
}
