using System.Threading.Tasks;
using Weapsy.Domain.Languages;
using Weapsy.Domain.Menus;
using Weapsy.Reporting.Menus;
using Weapsy.Reporting.Menus.Queries;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Weapsy.Reporting.Roles.Queries;
using System.Collections.Generic;
using Weapsy.Data.Entities;
using Weapsy.Domain.Roles.DefaultRoles;
using Weapsy.Framework.Queries;

namespace Weapsy.Data.Reporting.Menus
{
    public class GetItemForAdminHandler : IQueryHandlerAsync<GetItemForAdmin, MenuItemAdminModel>
    {
        private readonly IContextFactory _contextFactory;
        private readonly IQueryDispatcher _queryDispatcher;

        public GetItemForAdminHandler(IContextFactory contextFactory, IQueryDispatcher queryDispatcher)
        {
            _contextFactory = contextFactory;
            _queryDispatcher = queryDispatcher;
        }

        public async Task<MenuItemAdminModel> RetrieveAsync(GetItemForAdmin query)
        {
            using (var context = _contextFactory.Create())
            {
                var menu = await context.Menus.FirstOrDefaultAsync(x => x.SiteId == query.SiteId && x.Id == query.MenuId && x.Status != MenuStatus.Deleted);

                if (menu == null)
                    return new MenuItemAdminModel();

                var menuItem = await context.MenuItems
                    .Include(x => x.MenuItemLocalisations)
                    .Include(x => x.MenuItemPermissions)
                    .FirstOrDefaultAsync(x => x.MenuId == query.MenuId && x.Id == query.MenuItemId && x.Status != MenuItemStatus.Deleted);

                if (menuItem == null)
                    return new MenuItemAdminModel();

                var result = new MenuItemAdminModel
                {
                    Id = menuItem.Id,
                    Type = menuItem.Type,
                    PageId = menuItem.PageId,
                    Link = menuItem.Link,
                    Text = menuItem.Text,
                    Title = menuItem.Title
                };

                var languages = await context.Languages
                    .Where(x => x.SiteId == query.SiteId && x.Status != LanguageStatus.Deleted)
                    .OrderBy(x => x.SortOrder)
                    .ToListAsync();

                foreach (var language in languages)
                {
                    var text = string.Empty;
                    var title = string.Empty;

                    var existingLocalisation = menuItem.MenuItemLocalisations.FirstOrDefault(x => x.LanguageId == language.Id);

                    if (existingLocalisation != null)
                    {
                        text = existingLocalisation.Text;
                        title = existingLocalisation.Title;
                    }

                    result.MenuItemLocalisations.Add(new MenuItemAdminModel.MenuItemLocalisation
                    {
                        MenuItemId = menuItem.Id,
                        LanguageId = language.Id,
                        LanguageName = language.Name,
                        LanguageStatus = language.Status,
                        Text = text,
                        Title = title
                    });
                }

                foreach (var role in await _queryDispatcher.DispatchAsync<GetAllRoles, IEnumerable<Role>>(new GetAllRoles()))
                {
                    bool selected = menuItem.MenuItemPermissions.FirstOrDefault(x => x.RoleId == role.Id) != null;

                    result.MenuItemPermissions.Add(new MenuItemAdminModel.MenuItemPermission
                    {
                        MenuItemId = menuItem.Id,
                        RoleId = role.Id.ToString(),
                        RoleName = role.Name,
                        Selected = selected || role.Name == Administrator.Name,
                        Disabled = role.Name == Administrator.Name
                    });
                }

                return result;
            }
        }
    }
}
