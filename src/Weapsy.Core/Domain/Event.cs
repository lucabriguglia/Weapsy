using System;

namespace Weapsy.Core.Domain
{
    public class Event : IEvent
    {
        public Guid AggregateRootId { get; set; }
        public int Version { get; set; }
        public DateTime TimeStamp { get; private set; } = DateTime.UtcNow;
        public string UserId { get; set; }
    }
}
