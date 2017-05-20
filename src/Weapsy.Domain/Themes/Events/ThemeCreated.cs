using Weapsy.Framework.Domain;

namespace Weapsy.Domain.Themes.Events
{
    public class ThemeCreated : DomainEvent
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Folder { get; set; }
        public ThemeStatus Status { get; set; }
        public int SortOrder { get; set; }
    }
}
