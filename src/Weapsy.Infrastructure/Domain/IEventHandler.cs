namespace Weapsy.Infrastructure.Domain
{
    public interface IEventHandler<in TEvent> where TEvent : IDomainEvent
    {
        void Handle(TEvent @event);
    }
}
