using System.Threading.Tasks;

namespace Weapsy.Core.Domain
{
    public interface IEventHandler<in TEvent> where TEvent : IEvent
    {
        Task Handle(TEvent @event);
    }
}
