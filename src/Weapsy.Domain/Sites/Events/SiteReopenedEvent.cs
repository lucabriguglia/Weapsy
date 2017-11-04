using Weapsy.Framework.Domain;

namespace Weapsy.Domain.Sites.Events
{
    public class SiteReopenedEvent : DomainEvent
    {
        public string Name { get; set; }
    }
}