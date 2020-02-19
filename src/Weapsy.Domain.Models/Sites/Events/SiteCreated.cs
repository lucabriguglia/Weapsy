using Weapsy.Core.Domain;

namespace Weapsy.Domain.Models.Sites.Events
{
    public class SiteCreated : Event
    {
        public string Name { get; set; }
    }
}
