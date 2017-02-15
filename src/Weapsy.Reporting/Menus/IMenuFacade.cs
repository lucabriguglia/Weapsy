using System;
using System.Collections.Generic;

namespace Weapsy.Reporting.Menus
{
    public interface IMenuFacade
    {
        MenuViewModel GetByName(Guid siteId, string name, Guid languageId = new Guid());
        IEnumerable<MenuAdminModel> GetAllForAdmin(Guid siteId);
        MenuAdminModel GetForAdmin(Guid siteId, Guid id);
        MenuItemAdminModel GetItemForAdmin(Guid siteId, Guid menuId, Guid menuItemId);
        MenuItemAdminModel GetDefaultItemForAdmin(Guid siteId, Guid menuId);
        IEnumerable<MenuItemAdminListModel> GetMenuItemsForAdminList(Guid siteId, Guid id);               
    }
}
