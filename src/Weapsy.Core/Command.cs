using System;

namespace Weapsy.Core
{
    public abstract class Command : ICommand
    {
        public Guid SiteId { get; set; }
        public Guid UserId { get; set; }
        public DateTime TimeStamp { get; set; } = DateTime.UtcNow;
    }
}