using Weapsy.Framework.Domain;

namespace Weapsy.Domain.Sites.Events
{
    public class SiteClosedEvent : DomainEvent
    {
        public string Name { get; set; }
    }
}