using System;
using System.Collections.Generic;
using Weapsy.Framework.Domain;

namespace Weapsy.Domain.Menus.Events
{
    public class MenuItemsReordered : DomainEvent
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
