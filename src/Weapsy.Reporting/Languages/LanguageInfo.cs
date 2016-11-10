using System;

namespace Weapsy.Reporting.Languages
{
    public class LanguageInfo
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string CultureName { get; set; }
        public string Url { get; set; }
        public int SortOrder { get; set; }
    }
}
