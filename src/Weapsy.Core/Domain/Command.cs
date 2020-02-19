using System;

namespace Weapsy.Core.Domain
{
    public abstract class Command : ICommand
    {
        public Guid SiteId { get; set; }
        public string UserId { get; set; }
        public DateTime TimeStamp { get; set; } = DateTime.UtcNow;
    }
}