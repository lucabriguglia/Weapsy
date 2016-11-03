using System;
using Weapsy.Infrastructure.Domain;

namespace Weapsy.Domain.Languages.Events
{
    public class LanguageReordered : Event
    {
        public Guid SiteId { get; set; }
        public int SortOrder { get; set; }
    }
}
