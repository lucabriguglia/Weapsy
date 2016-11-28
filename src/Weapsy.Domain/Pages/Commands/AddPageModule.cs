using System;
using System.Collections.Generic;

namespace Weapsy.Domain.Pages.Commands
{
    public class AddPageModule : BaseSiteCommand
    {
        public Guid PageId { get; set; }
        public Guid ModuleId { get; set; }
        public Guid PageModuleId { get; set; }
        public string Title { get; set; }
        public string Zone { get; set; }
        public int SortOrder { get; set; }
        public IList<PageModulePermission> PageModulePermissions { get; set; } = new List<PageModulePermission>();
    }
}