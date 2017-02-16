using System;
using System.Threading.Tasks;
using Weapsy.Domain.Languages;
using Weapsy.Domain.Pages;
using Weapsy.Infrastructure.Identity;
using Weapsy.Infrastructure.Queries;
using Weapsy.Reporting.Pages;
using Weapsy.Reporting.Pages.Queries;
using System.Linq;
using Weapsy.Data.Identity;
using Weapsy.Domain.Menus;
using Microsoft.EntityFrameworkCore;

namespace Weapsy.Data.Reporting.Pages
{
    public class GetDefaultForAdminHandler : IQueryHandlerAsync<GetDefaultForAdmin, PageAdminModel>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly IRoleService _roleService;

        public GetDefaultForAdminHandler(IDbContextFactory contextFactory, IRoleService roleService)
        {
            _contextFactory = contextFactory;
            _roleService = roleService;
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

                foreach (var role in _roleService.GetAllRoles())
                {
                    var pagePermission = new PagePermissionModel
                    {
                        RoleId = role.Id,
                        RoleName = role.Name,
                        Disabled = role.Name == DefaultRoleNames.Administrator
                    };

                    foreach (PermissionType permisisonType in Enum.GetValues(typeof(PermissionType)))
                    {
                        pagePermission.PagePermissionTypes.Add(new PagePermissionTypeModel
                        {
                            Type = permisisonType,
                            Selected = role.Name == DefaultRoleNames.Administrator
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
