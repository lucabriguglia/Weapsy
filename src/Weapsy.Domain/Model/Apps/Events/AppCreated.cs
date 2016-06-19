using Weapsy.Core.Domain;

namespace Weapsy.Domain.Model.Apps.Events
{
    public class AppCreated : Event
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Folder { get; set; }
        public AppStatus Status { get; set; }
    }
}
