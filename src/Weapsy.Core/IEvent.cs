using System;

namespace Weapsy.Core
{
    public interface IEvent
    {
        Guid SiteId { get; set; }
        Guid UserId { get; set; }
        DateTime TimeStamp { get; set; }
    }
}