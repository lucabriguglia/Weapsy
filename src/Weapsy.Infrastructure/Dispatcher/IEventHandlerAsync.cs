using System.Threading.Tasks;

namespace Weapsy.Infrastructure.Dispatcher
{
    public interface IEventHandlerAsync<in TEvent> where TEvent : IEvent
    {
        Task HandleAsync(TEvent @event);
    }
}
