using Weapsy.Core.Domain;

namespace Weapsy.Core.Dispatcher
{
    public interface IEventPublisher
    {
        void Publish<TEvent, TAggregate>(TEvent @event) 
            where TEvent : IEvent
            where TAggregate : IAggregateRoot;
    }
}
