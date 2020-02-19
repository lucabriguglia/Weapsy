using System;

namespace Weapsy.Core
{
    public interface ICommand
    {
        Guid SiteId { get; set; }
        Guid UserId { get; set; }
        DateTime TimeStamp { get; set; }
    }
}
