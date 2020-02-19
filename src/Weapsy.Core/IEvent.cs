using System;

namespace Weapsy.Core
{
    public interface IEvent
    {
        Guid SiteId { get; set; }
        string UserId { get; set; }
        DateTime TimeStamp { get; set; }
    }
}