using System;

namespace Weapsy.Core.Domain
{
    public interface IEvent
    {
        Guid SiteId { get; set; }
        string UserId { get; set; }
        DateTime TimeStamp { get; set; }
    }
}