using System;
using Weapsy.Domain.Languages;

namespace Weapsy.Reporting.Languages
{
    public class LanguageAdminModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string CultureName { get; set; }
        public string Url { get; set; }
        public int SortOrder { get; set; }
        public LanguageStatus Status { get; set; }
    }
}
