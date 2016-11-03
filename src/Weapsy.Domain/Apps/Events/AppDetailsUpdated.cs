using Weapsy.Infrastructure.Domain;

namespace Weapsy.Domain.Apps.Events
{
    public class AppDetailsUpdated : Event
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Folder { get; set; }
    }
}
