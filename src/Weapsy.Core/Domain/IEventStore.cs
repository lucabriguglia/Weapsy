using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Weapsy.Core.Domain
{
    public interface IEventStore
    {
        void SaveEvent<TAggregate>(IEvent @event) where TAggregate : IAggregateRoot;
        Task<IEnumerable<IEvent>> GetEvents(Guid aggregateId);
    }
}
