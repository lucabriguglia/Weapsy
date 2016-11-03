using System;
using System.Collections.Generic;

namespace Weapsy.Domain.Pages.Commands
{
    public class SetPageModulePermissions : BaseSiteCommand
    {
        public Guid Id { get; set; }
        public Guid PageModuleId { get; set; }
        public IList<PageModulePermission> PageModulePermissions { get; set; } = new List<PageModulePermission>();
    }
}
