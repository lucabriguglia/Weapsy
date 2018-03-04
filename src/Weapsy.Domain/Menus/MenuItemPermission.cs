using System;

namespace Weapsy.Domain.Menus
{
    public class MenuItemPermission
    {
        public Guid MenuItemId { get; set; }  
        public Guid RoleId { get; set; }
    }
}
