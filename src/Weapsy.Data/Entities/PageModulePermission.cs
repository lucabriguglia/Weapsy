using System;
using Weapsy.Domain.Pages;

namespace Weapsy.Data.Entities
{
    public class PageModulePermission
    {
        public Guid PageModuleId { get; set; }  
        public PermissionType Type { get; set; }
        public Guid RoleId { get; set; }
    }
}
