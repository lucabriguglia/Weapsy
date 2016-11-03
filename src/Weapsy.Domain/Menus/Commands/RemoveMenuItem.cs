using System;

namespace Weapsy.Domain.Menus.Commands
{
    public class RemoveMenuItem : BaseSiteCommand
    {
        public Guid MenuId { get; set; }
        public Guid MenuItemId { get; set; }
    }
}
