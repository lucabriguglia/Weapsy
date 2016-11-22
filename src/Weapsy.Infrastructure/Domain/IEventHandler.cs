using System.Threading.Tasks;

namespace Weapsy.Infrastructure.Domain
{
    public interface IEventHandler<in TEvent> where TEvent : IEvent
    {
        Task Handle(TEvent @event);
    }
}
