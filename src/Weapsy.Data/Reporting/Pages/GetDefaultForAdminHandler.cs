using System;
using System.Threading.Tasks;
using Weapsy.Domain.Languages;
using Weapsy.Domain.Pages;
using Weapsy.Reporting.Pages;
using Weapsy.Reporting.Pages.Queries;
using System.Linq;
using Weapsy.Domain.Menus;
using Microsoft.EntityFrameworkCore;
using Weapsy.Reporting.Roles.Queries;
using System.Collections.Generic;
using Weapsy.Data.Entities;
using Weapsy.Domain.Roles.DefaultRoles;
using Weapsy.Framework.Queries;

namespace Weapsy.Data.Reporting.Pages
{
    public class GetDefaultForAdminHandler : IQueryHandlerAsync<GetDefaultForAdmin, PageAdminModel>
    {
        private readonly IContextFactory _contextFactory;
        private readonly IQueryDispatcher _queryDispatcher;

        public GetDefaultForAdminHandler(IContextFactory contextFactory, IQueryDispatcher queryDispatcher)
        {
            _contextFactory = contextFactory;
            _queryDispatcher = queryDispatcher;
        }

        public async Task<PageAdminModel> RetrieveAsync(GetDefaultForAdmin query)
        {
            using (var context = _contextFactory.Create())
            {
                var result = new PageAdminModel();

                var languages = await context.Languages
                    .Where(x => x.SiteId == query.SiteId && x.Status != LanguageStatus.Deleted)
                    .OrderBy(x => x.SortOrder)
                    .ToListAsync();

                foreach (var language in languages)
                {
                    result.PageLocalisations.Add(new PageLocalisationAdminModel
                    {
                        LanguageId = language.Id,
                        LanguageName = language.Name,
                        LanguageStatus = language.Status
                    });
                }

                foreach (var role in await _queryDispatcher.DispatchAsync<GetAllRoles, IEnumerable<Role>>(new GetAllRoles()))
                {
                    var pagePermission = new PagePermissionModel
                    {
                        RoleId = role.Id,
                        RoleName = role.Name,
                        Disabled = role.Name == Administrator.Name
                    };

                    foreach (PermissionType permisisonType in Enum.GetValues(typeof(PermissionType)))
                    {
                        pagePermission.PagePermissionTypes.Add(new PagePermissionTypeModel
                        {
                            Type = permisisonType,
                            Selected = role.Name == Administrator.Name
                        });
                    }

                    result.PagePermissions.Add(pagePermission);
                }

                var menus = context.Menus.Where(x => x.SiteId == query.SiteId && x.Status == MenuStatus.Active)
                        .Select(menu => new MenuModel
                        {
                            MenuId = menu.Id,
                            MenuName = menu.Name,
                            Selected = false
                        });

                result.Menus.AddRange(menus);

                return result;
            }
        }
    }
}
