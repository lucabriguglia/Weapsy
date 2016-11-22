using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Weapsy.Infrastructure.Domain
{
    public interface IEventStore
    {
        void SaveEvent<TAggregate>(IEvent @event) where TAggregate : IAggregateRoot;
        Task SaveEventAsync<TAggregate>(IEvent @event) where TAggregate : IAggregateRoot;
        Task<IEnumerable<IEvent>> GetEventsAsync(Guid aggregateId);
    }
}
