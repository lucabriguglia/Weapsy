using System;
using System.Collections.Generic;
using Weapsy.Core.Domain;

namespace Weapsy.Domain.Menus.Events
{
    public class MenuItemPermissionsSet : Event
    {
        public Guid SiteId { get; set; }
        public Guid MenuItemId { get; set; }
        public IList<MenuItemPermission> MenuItemPermissions { get; set; }
    }
}
    