using Weapsy.Infrastructure.Domain;

namespace Weapsy.Domain.Sites.Events
{
    public class SiteDeleted : Event
    {
        public string Name { get; set; }
    }
}
