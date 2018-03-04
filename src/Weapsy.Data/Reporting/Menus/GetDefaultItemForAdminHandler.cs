using System.Threading.Tasks;
using Weapsy.Domain.Languages;
using Weapsy.Domain.Menus;
using Weapsy.Reporting.Menus;
using Weapsy.Reporting.Menus.Queries;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Weapsy.Reporting.Roles.Queries;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Weapsy.Cqrs;
using Weapsy.Data.Entities;
using Weapsy.Data.TempIdentity;
using Weapsy.Domain.Roles.DefaultRoles;
using Weapsy.Cqrs.Queries;

namespace Weapsy.Data.Reporting.Menus
{
    public class GetDefaultItemForAdminHandler : IQueryHandlerAsync<GetDefaultItemForAdmin, MenuItemAdminModel>
    {
        private readonly IContextFactory _contextFactory;
        private readonly IDispatcher _queryDispatcher;

        public GetDefaultItemForAdminHandler(IContextFactory contextFactory, IDispatcher queryDispatcher)
        {
            _contextFactory = contextFactory;
            _queryDispatcher = queryDispatcher;
        }

        public async Task<MenuItemAdminModel> RetrieveAsync(GetDefaultItemForAdmin query)
        {
            using (var context = _contextFactory.Create())
            {
                var menu = await context.Menus.FirstOrDefaultAsync(x => x.SiteId == query.SiteId && x.Id == query.MenuId && x.Status != MenuStatus.Deleted);

                if (menu == null)
                    return new MenuItemAdminModel();

                var result = new MenuItemAdminModel();

                var languages = await context.Languages
                    .Where(x => x.SiteId == query.SiteId && x.Status != LanguageStatus.Deleted)
                    .OrderBy(x => x.SortOrder)
                    .ToListAsync();

                foreach (var language in languages)
                {
                    result.MenuItemLocalisations.Add(new MenuItemAdminModel.MenuItemLocalisation
                    {
                        LanguageId = language.Id,
                        LanguageName = language.Name,
                        LanguageStatus = language.Status,
                        Text = string.Empty,
                        Title = string.Empty
                    });
                }

                foreach (var role in await _queryDispatcher.GetResultAsync<GetAllRoles, IEnumerable<ApplicationRole>>(new GetAllRoles()))
                {
                    result.MenuItemPermissions.Add(new MenuItemAdminModel.MenuItemPermission
                    {
                        RoleId = role.Id.ToString(),
                        RoleName = role.Name,
                        Selected = role.Name == Administrator.Name,
                        Disabled = role.Name == Administrator.Name
                    });
                }

                return result;
            }
        }
    }
}
