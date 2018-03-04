using System;
using System.Collections.Generic;

namespace Weapsy.Domain.Pages.Commands
{
    public class UpdatePageModuleDetails : BaseSiteCommand
    {
        public Guid PageId { get; set; }
        public Guid ModuleId { get; set; }
        public string Title { get; set; }
        public bool InheritPermissions { get; set; }
        public IList<PageModuleLocalisation> PageModuleLocalisations { get; set; } = new List<PageModuleLocalisation>();
        public IList<PageModulePermission> PageModulePermissions { get; set; } = new List<PageModulePermission>();
    }
}
