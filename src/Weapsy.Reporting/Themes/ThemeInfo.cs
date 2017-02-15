using System;

namespace Weapsy.Reporting.Themes
{
    public class ThemeInfo
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Folder { get; set; }
        public int SortOrder { get; set; }
    }
}
