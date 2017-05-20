using Weapsy.Framework.Domain;

namespace Weapsy.Domain.Themes.Events
{
    public class ThemeReordered : DomainEvent
    {
        public int SortOrder { get; set; }
    }
}
