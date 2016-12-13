using System.Threading.Tasks;

namespace Weapsy.Infrastructure.Domain
{
    public interface IEventHandlerAsync<in TEvent> where TEvent : IDomainEvent
    {
        Task HandleAsync(TEvent @event);
    }
}
