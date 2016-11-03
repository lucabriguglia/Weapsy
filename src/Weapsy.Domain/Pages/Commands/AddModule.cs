using System;
using System.Collections.Generic;

namespace Weapsy.Domain.Pages.Commands
{
    public class AddModule : BaseSiteCommand
    {
        public Guid PageId { get; set; }
        public Guid ModuleTypeId { get; set; }
        public string Zone { get; set; }
        public int SortOrder { get; set; }
        public string Title { get; set; }
        public IList<string> ViewPermissionRoleIds { get; set; }
    }
}
