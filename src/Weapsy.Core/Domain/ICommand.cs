using System;

namespace Weapsy.Core.Domain
{
    public interface ICommand
    {
        Guid SiteId { get; set; }
        string UserId { get; set; }
        DateTime TimeStamp { get; set; }
    }
}
