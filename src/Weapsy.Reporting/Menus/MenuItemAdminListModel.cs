using System;
using System.Collections.Generic;
using Weapsy.Domain.Menus;

namespace Weapsy.Reporting.Menus
{
    public class MenuItemAdminListModel
    {
        public Guid Id { get; set; }
        public Guid ParentId { get; set; }
        public int SortOrder { get; set; }
        public string Text { get; set; }
        public MenuItemType Type { get; set; }

        public List<MenuItemAdminListModel> MenuItems { get; set; } = new List<MenuItemAdminListModel>();
    }
}
