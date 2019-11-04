using System;

namespace Weapsy.Domain.Pages
{
    public class PageModulePermission
    {
        public Guid PageModuleId { get; set; }  
        public PermissionType Type { get; set; }
        public Guid RoleId { get; set; }
    }
}
