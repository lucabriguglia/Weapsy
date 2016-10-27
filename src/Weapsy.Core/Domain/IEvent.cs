using System;

namespace Weapsy.Core.Domain
{
    public interface IEvent
    {
        Guid AggregateRootId { get; set; }
        int Version { get; set; }
        DateTime TimeStamp { get; set; }
        string UserId { get; set; }
    }
}
