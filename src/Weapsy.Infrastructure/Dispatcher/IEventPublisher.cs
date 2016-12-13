using System.Threading.Tasks;
using Weapsy.Infrastructure.Domain;

namespace Weapsy.Infrastructure.Dispatcher
{
    public interface IEventPublisher
    {
        void Publish<TEvent>(TEvent @event) where TEvent : IDomainEvent;
        Task PublishAsync<TEvent>(TEvent @event) where TEvent : IDomainEvent;
    }
}
