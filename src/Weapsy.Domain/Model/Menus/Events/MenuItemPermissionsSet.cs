using System;
using System.Collections.Generic;
using Weapsy.Core.Domain;

namespace Weapsy.Domain.Model.Menus.Events
{
    public class MenuItemPermissionsSet : Event
    {
        public Guid SiteId { get; set; }
        public Guid MenuItemId { get; set; }
        public IList<MenuItemPermission> MenuItemPermissions { get; set; }
    }
}
    