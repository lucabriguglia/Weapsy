using System;
using Weapsy.Core.Domain;

namespace Weapsy.Domain.Model.Languages.Events
{
    public class LanguageReordered : Event
    {
        public Guid SiteId { get; set; }
        public int SortOrder { get; set; }
    }
}
