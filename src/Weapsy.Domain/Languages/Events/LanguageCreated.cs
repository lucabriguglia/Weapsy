using System;
using Weapsy.Core.Domain;

namespace Weapsy.Domain.Languages.Events
{
    public class LanguageCreated : Event
    {
        public Guid SiteId { get; set; }
        public string Name { get; set; }
        public string CultureName { get; set; }
        public string Url { get; set; }
        public int SortOrder { get; set; }
        public LanguageStatus Status { get; set; }
    }
}
