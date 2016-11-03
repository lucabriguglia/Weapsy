using System;

namespace Weapsy.Infrastructure.Domain
{
    public interface IEvent
    {
        Guid AggregateRootId { get; set; }
        int Version { get; set; }
        DateTime TimeStamp { get; set; }
        string UserId { get; set; }
    }
}
