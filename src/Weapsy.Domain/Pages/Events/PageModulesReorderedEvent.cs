using System;
using System.Collections.Generic;
using Weapsy.Framework.Domain;

namespace Weapsy.Domain.Pages.Events
{
    public class PageModulesReorderedEvent : DomainEvent
    {
        public Guid SiteId { get; set; }
        public IList<PageModule> PageModules { get; set; }

        public class PageModule
        {
            public Guid ModuleId { get; set; }
            public string Zone { get; set; }
            public int SortOrder { get; set; }
        }
    }
}
