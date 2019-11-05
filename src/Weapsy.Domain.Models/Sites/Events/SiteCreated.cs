using Kledex.Domain;

namespace Weapsy.Domain.Models.Sites.Events
{
    public class SiteCreated : DomainEvent
    {
        public string Name { get; set; }
    }
}
