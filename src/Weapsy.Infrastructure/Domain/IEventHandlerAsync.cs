using System.Threading.Tasks;

namespace Weapsy.Infrastructure.Domain
{
    public interface IEventHandlerAsync<in TEvent> where TEvent : IEvent
    {
        Task HandleAsync(TEvent @event);
    }
}
