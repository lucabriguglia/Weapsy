using System;
using System.Threading.Tasks;
using Weapsy.Core.DependencyResolver;
using Weapsy.Core.Domain;

namespace Weapsy.Core.Dispatcher
{
    public class EventPublisher : IEventPublisher
    {
        private readonly IResolver _resolver;
        private readonly IEventStore _eventStore;

        public EventPublisher(IResolver resolver,
            IEventStore eventStore)
        {
            _resolver = resolver;
            _eventStore = eventStore;
        }

        public void Publish<TEvent, TAggregate>(TEvent @event)
            where TEvent : IEvent
            where TAggregate : IAggregateRoot
        {
            if (@event == null)
                throw new ArgumentNullException(nameof(@event));

            _eventStore.SaveEvent<TAggregate>(@event);

            var eventHandlers = _resolver.ResolveAll<IEventHandler<TEvent>>();

            foreach (var handler in eventHandlers)
                Task.Run(() => handler.Handle(@event));
        }
    }
}
