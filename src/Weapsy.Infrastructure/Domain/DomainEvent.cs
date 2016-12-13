using System;
using Weapsy.Infrastructure.Dispatcher;

namespace Weapsy.Infrastructure.Domain
{
    public class DomainEvent : IDomainEvent, IEvent
    {
        public Guid AggregateRootId { get; set; }
        public int Version { get; set; }
        public DateTime TimeStamp { get; set; } = DateTime.UtcNow;
        public string UserId { get; set; }
    }
}
