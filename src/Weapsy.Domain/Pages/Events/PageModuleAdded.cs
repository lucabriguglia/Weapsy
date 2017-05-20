using System;
using System.Collections.Generic;
using Weapsy.Framework.Domain;

namespace Weapsy.Domain.Pages.Events
{
    public class PageModuleAdded : DomainEvent
    {
        public Guid SiteId { get; set; }
        public Guid PageModuleId { get; set; }
        public Guid ModuleId { get; set; }
        public string Title { get; set; }
        public string Zone { get; set; }
        public int SortOrder { get; set; }
        public PageModuleStatus PageModuleStatus { get; set; }
        public ICollection<ReorderedModule> ReorderedModules { get; set; } = new List<ReorderedModule>();

        public class ReorderedModule
        {
            public Guid ModuleId { get; set; }
            public int SortOrder { get; set; }
        }
    }
}
