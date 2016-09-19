using System;
using Weapsy.Core.Domain;

namespace Weapsy.Domain.Languages.Events
{
    public class LanguageDetailsUpdated : Event
    {
        public Guid SiteId { get; set; }
        public string Name { get; set; }
        public string CultureName { get; set; }
        public string Url { get; set; }
    }
}
