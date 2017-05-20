using Weapsy.Framework.Domain;

namespace Weapsy.Domain.Sites.Events
{
    public class SiteCreated : DomainEvent
    {
        public string Name { get; set; }
        public SiteStatus Status { get; set; }
    }
}
