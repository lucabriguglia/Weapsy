using System;

namespace Weapsy.Infrastructure.Domain
{
    public class Event : IEvent
    {
        public Guid AggregateRootId { get; set; }
        public int Version { get; set; }
        public DateTime TimeStamp { get; set; } = DateTime.UtcNow;
        public string UserId { get; set; }
    }
}
