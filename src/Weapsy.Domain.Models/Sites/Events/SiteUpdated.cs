using Weapsy.Core;

namespace Weapsy.Domain.Models.Sites.Events
{
    public class SiteUpdated : Event
    {
        public string Name { get; set; }
    }
}
