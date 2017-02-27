using System;
using System.Collections.Generic;
using PageStatus = Weapsy.Domain.Pages.PageStatus;

namespace Weapsy.Data.Entities
{
    public class Page
    {
        public Guid Id { get; set; }
        public Guid SiteId { get; set; }
        public string Name { get; set; }
        public PageStatus Status { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeywords { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public ICollection<PageLocalisation> PageLocalisations { get; set; } = new HashSet<PageLocalisation>();
        public ICollection<PageModule> PageModules { get; set; } = new HashSet<PageModule>();
        public ICollection<PagePermission> PagePermissions { get; set; } = new HashSet<PagePermission>();

        public virtual Site Site { get; set; }
    }
}