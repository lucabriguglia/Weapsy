using Weapsy.Infrastructure.Domain;

namespace Weapsy.Domain.Sites.Events
{
    public class SiteReopened : Event
    {
        public string Name { get; set; }
    }
}