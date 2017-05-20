using System.Threading.Tasks;

namespace Weapsy.Framework.Events
{
    public interface IEventPublisher
    {
        void Publish<TEvent>(TEvent @event) where TEvent : IEvent;
        Task PublishAsync<TEvent>(TEvent @event) where TEvent : IEvent;
    }
}
