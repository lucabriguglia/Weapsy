using Weapsy.Cqrs.Domain;

namespace Weapsy.Domain.Sites.Events
{
    public class SiteClosed : DomainEvent
    {
        public string Name { get; set; }
    }
}