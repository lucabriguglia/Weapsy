using System;

namespace Weapsy.Domain.Menus.Commands
{
    public class RemoveMenuItemCommand : BaseSiteCommand
    {
        public Guid MenuId { get; set; }
        public Guid MenuItemId { get; set; }
    }
}
