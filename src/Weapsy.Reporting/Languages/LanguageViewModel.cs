using System;

namespace Weapsy.Reporting.Languages
{
    public class LanguageViewModel
    {
        public Guid LanguageId { get; set; }
        public string Name { get; set; }
        public string CultureName { get; set; }
        public string Url { get; set; }
        public int SortOrder { get; set; }
    }
}
