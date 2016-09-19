using Weapsy.Core.Domain;

namespace Weapsy.Domain.Themes.Events
{
    public class ThemeReordered : Event
    {
        public int SortOrder { get; set; }
    }
}
