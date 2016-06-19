using System;
using System.Collections.Generic;
using Weapsy.Core.Domain;

namespace Weapsy.Domain.Model.Menus.Events
{
    public class MenuItemsReordered : Event
    {
        public Guid SiteId { get; set; }
        public string Name { get; set; }
        public IList<MenuItem> MenuItems { get; set; }

        public class MenuItem
        {
            public Guid Id { get; set; }
            public Guid ParentId { get; set; }
            public int SortOrder { get; set; }
        }
    }
}
