using System;
using Weapsy.Infrastructure.Dispatcher;

namespace Weapsy.Infrastructure.Domain
{
    public interface IDomainEvent : IEvent
    {
        Guid AggregateRootId { get; set; }
        int Version { get; set; }
        DateTime TimeStamp { get; set; }
        string UserId { get; set; }
    }
}
