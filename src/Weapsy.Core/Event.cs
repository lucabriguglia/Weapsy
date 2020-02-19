using System;

namespace Weapsy.Core
{
    public abstract class Event : IEvent
    {
        public Guid SiteId { get; set; }
        public string UserId { get; set; }
        public DateTime TimeStamp { get; set; } = DateTime.UtcNow;
    }
}