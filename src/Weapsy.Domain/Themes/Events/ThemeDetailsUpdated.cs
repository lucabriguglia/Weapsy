using Weapsy.Cqrs.Domain;

namespace Weapsy.Domain.Themes.Events
{
    public class ThemeDetailsUpdated : DomainEvent
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Folder { get; set; }
    }
}
