using Weapsy.Core.Domain;

namespace Weapsy.Domain.Model.Themes.Events
{
    public class ThemeReordered : Event
    {
        public int SortOrder { get; set; }
    }
}
