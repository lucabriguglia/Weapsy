using System;
using System.Collections.Generic;
using Weapsy.Domain.Model.Pages;

namespace Weapsy.Domain.Data.Entities
{
    public class PageModule : IDbEntity
    {
        public Guid Id { get; set; }
        public Guid PageId { get; set; }
        public Guid ModuleId { get; set; }
        public string Title { get; set; }
        public string Zone { get; set; }
        public int SortOrder { get; set; }
        public PageModuleStatus Status { get; set; }

        public virtual ICollection<PageModuleLocalisation> PageModuleLocalisations { get; set; } = new List<PageModuleLocalisation>();
    }
}