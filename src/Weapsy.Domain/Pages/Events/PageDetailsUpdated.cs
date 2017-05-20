using System;
using System.Collections.Generic;
using Weapsy.Framework.Domain;

namespace Weapsy.Domain.Pages.Events
{
    public class PageDetailsUpdated : DomainEvent
    {
        public Guid SiteId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeywords { get; set; }
        public IEnumerable<PageLocalisation> PageLocalisations { get; set; }
        public IEnumerable<PagePermission> PagePermissions { get; set; }
    }
}
