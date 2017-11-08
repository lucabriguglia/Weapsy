using System;
using System.Collections.Generic;

namespace Weapsy.Domain.Menus.Commands
{
    public class SetMenuItemPermissionsCommand : BaseSiteCommand
    {
        public Guid MenuId { get; set; }
        public Guid MenuItemId { get; set; }
        public IList<MenuItemPermission> MenuItemPermissions { get; set; }
    }
}
    