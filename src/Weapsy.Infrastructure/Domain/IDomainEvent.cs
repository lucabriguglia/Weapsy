using System;
using Weapsy.Infrastructure.Events;

namespace Weapsy.Infrastructure.Domain
{
    public interface IDomainEvent : IEvent
    {
        Guid AggregateRootId { get; set; }
        int Version { get; set; }
    }
}
