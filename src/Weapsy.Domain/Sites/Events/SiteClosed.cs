using Weapsy.Infrastructure.Domain;

namespace Weapsy.Domain.Sites.Events
{
    public class SiteClosed : Event
    {
        public string Name { get; set; }
    }
}