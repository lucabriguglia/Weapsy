using System;
using System.Collections.Generic;

namespace Weapsy.Domain.Menus.Commands
{
    public class ReorderMenuItems : BaseSiteCommand
    {
        public Guid Id { get; set; }
        public IList<MenuItem> MenuItems { get; set; } = new List<MenuItem>();

        public class MenuItem
        {
            public Guid Id { get; set; }
            public Guid ParentId { get; set; }
        }
    }
}
