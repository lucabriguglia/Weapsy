using System;
using System.Collections.Generic;
using Weapsy.Cqrs.Domain;

namespace Weapsy.Domain.Pages.Events
{
    public class PageModuleDetailsUpdated : DomainEvent
    {
        public Guid SiteId { get; set; }
        public Guid PageId { get; set; }
        public Guid PageModuleId { get; set; }
        public string Title { get; set; }
        public bool InheritPermissions { get; set; }
        public IList<PageModuleLocalisation> PageModuleLocalisations { get; set; } = new List<PageModuleLocalisation>();
        public IList<PageModulePermission> PageModulePermissions { get; set; } = new List<PageModulePermission>();
    }
}
