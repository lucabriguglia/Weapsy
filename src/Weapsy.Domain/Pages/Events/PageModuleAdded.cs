using System;
using System.Collections.Generic;
using Weapsy.Cqrs.Domain;

namespace Weapsy.Domain.Pages.Events
{
    public class PageModuleAdded : DomainEvent
    {
        public Guid SiteId { get; set; }
        public PageModule PageModule { get; set; }
        public ICollection<ReorderedModule> ReorderedModules { get; set; } = new List<ReorderedModule>();

        public class ReorderedModule
        {
            public Guid PageModuleId { get; set; }
            public string Zone { get; set; }
            public int SortOrder { get; set; }
        }
    }
}
