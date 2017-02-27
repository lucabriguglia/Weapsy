using System;
using System.Collections.Generic;
using Weapsy.Domain.Menus;

namespace Weapsy.Data.Entities
{
    public class MenuItem
    {
        public Guid Id { get; set; }
        public Guid MenuId { get; set; }
        public Guid ParentId { get; set; }
        public int SortOrder { get; set; }
        public MenuItemType Type { get; set; }
        public Guid PageId { get; set; }        
        public string Link { get; set; }
        public string Text { get; set; }
        public string Title { get; set; }
        public MenuItemStatus Status { get; set; }

        public ICollection<MenuItemLocalisation> MenuItemLocalisations { get; set; } = new HashSet<MenuItemLocalisation>();
        public ICollection<MenuItemPermission> MenuItemPermissions { get; set; } = new HashSet<MenuItemPermission>();

        public virtual Menu Menu { get; set; }
        //public virtual Page Page { get; set; }
    }
}
