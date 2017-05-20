using Weapsy.Framework.Domain;

namespace Weapsy.Domain.Apps.Events
{
    public class AppDetailsUpdated : DomainEvent
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Folder { get; set; }
    }
}
