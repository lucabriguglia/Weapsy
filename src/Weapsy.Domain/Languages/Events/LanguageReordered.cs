using System;
using Weapsy.Framework.Domain;

namespace Weapsy.Domain.Languages.Events
{
    public class LanguageReordered : DomainEvent
    {
        public Guid SiteId { get; set; }
        public int SortOrder { get; set; }
    }
}
