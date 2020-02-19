using System.Threading.Tasks;

namespace Weapsy.Core
{
    public interface IEventHandler<in TEvent> where TEvent : IEvent
    {
        Task HandleAsync(TEvent @event);
    }
}