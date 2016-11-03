using Weapsy.Infrastructure.Domain;

namespace Weapsy.Domain.Sites.Events
{
    public class SiteCreated : Event
    {
        public string Name { get; set; }
        public SiteStatus Status { get; set; }
    }
}
