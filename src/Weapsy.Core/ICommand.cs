using System;

namespace Weapsy.Core
{
    public interface ICommand
    {
        Guid SiteId { get; set; }
        string UserId { get; set; }
        DateTime TimeStamp { get; set; }
    }
}
