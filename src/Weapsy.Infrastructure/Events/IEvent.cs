using System;

namespace Weapsy.Infrastructure.Events
{
    public interface IEvent
    {
        DateTime TimeStamp { get; set; }
        Guid UserId { get; set; }
    }
}
