using System;
using System.Threading.Tasks;
using Weapsy.Infrastructure.DependencyResolver;

namespace Weapsy.Infrastructure.Dispatcher
{
    public class EventPublisher : IEventPublisher
    {
        private readonly IResolver _resolver;

        public EventPublisher(IResolver resolver)
        {
            _resolver = resolver;
        }

        public void Publish<TEvent>(TEvent @event) where TEvent : IEvent
        {
            if (@event == null)
                throw new ArgumentNullException(nameof(@event));

            var eventHandlers = _resolver.ResolveAll<IEventHandler<TEvent>>();

            foreach (var handler in eventHandlers)
                handler.Handle(@event);
        }

        public async Task PublishAsync<TEvent>(TEvent @event) where TEvent : IEvent
        {
            if (@event == null)
                throw new ArgumentNullException(nameof(@event));

            var eventHandlers = _resolver.ResolveAll<IEventHandlerAsync<TEvent>>();

            foreach (var handler in eventHandlers)
                await handler.HandleAsync(@event);
        }
    }
}
