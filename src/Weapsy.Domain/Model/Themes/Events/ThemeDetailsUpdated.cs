using Weapsy.Core.Domain;

namespace Weapsy.Domain.Model.Themes.Events
{
    public class ThemeDetailsUpdated : Event
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Folder { get; set; }
    }
}
