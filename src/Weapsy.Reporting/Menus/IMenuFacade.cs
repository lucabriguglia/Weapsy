using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Weapsy.Reporting.Menus
{
    public interface IMenuFacade
    {
        Task<MenuViewModel> GetByNameAsync(Guid siteId, string name, Guid languageId = new Guid());
        Task<IEnumerable<MenuAdminModel>> GetAllForAdminAsync(Guid siteId);
        Task<MenuAdminModel> GetForAdminAsync(Guid siteId, Guid id);
        Task<MenuItemAdminModel> GetItemForAdminAsync(Guid siteId, Guid menuId, Guid menuItemId);
        Task<IEnumerable<MenuItemAdminListModel>> GetMenuItemsForAdminListAsync(Guid siteId, Guid id);               
    }
}
