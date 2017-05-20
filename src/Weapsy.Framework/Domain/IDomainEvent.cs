using System;
using Weapsy.Framework.Events;

namespace Weapsy.Framework.Domain
{
    public interface IDomainEvent : IEvent
    {
        Guid AggregateRootId { get; set; }
        int Version { get; set; }
    }
}
