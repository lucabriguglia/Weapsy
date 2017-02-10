using System.Collections.Generic;
using System.Threading.Tasks;

namespace Weapsy.Infrastructure.Domain
{
    public interface IEventStore
    {
        void SaveEvent<TAggregate>(IDomainEvent @event) where TAggregate : IAggregateRoot;
        Task SaveEventAsync<TAggregate>(IDomainEvent @event) where TAggregate : IAggregateRoot;
        IEnumerable<IDomainEvent> GetEvents(string aggregateId);
        Task<IEnumerable<IDomainEvent>> GetEventsAsync(string aggregateId);
    }
}
