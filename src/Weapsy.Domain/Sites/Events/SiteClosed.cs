using Weapsy.Framework.Domain;

namespace Weapsy.Domain.Sites.Events
{
    public class SiteClosed : DomainEvent
    {
        public string Name { get; set; }
    }
}