using Weapsy.Core.Domain;

namespace Weapsy.Core.Dispatcher
{
    public interface IEventPublisher
    {
        void Publish<TEvent>(TEvent @event) where TEvent : IEvent;
    }
}
