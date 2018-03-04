using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Weapsy.Domain.Languages;
using Weapsy.Domain.Pages;
using Weapsy.Reporting.Pages;
using Weapsy.Reporting.Pages.Queries;
using System.Linq;
using Weapsy.Reporting.Roles.Queries;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Weapsy.Cqrs;
using Weapsy.Data.Entities;
using Weapsy.Data.TempIdentity;
using Weapsy.Domain.Roles.DefaultRoles;
using Weapsy.Cqrs.Queries;

namespace Weapsy.Data.Reporting.Pages
{
    public class GetForAdminHandler : IQueryHandlerAsync<GetForAdmin, PageAdminModel>
    {
        private readonly IContextFactory _contextFactory;
        private readonly IDispatcher _queryDispatcher;

        public GetForAdminHandler(IContextFactory contextFactory, IDispatcher queryDispatcher)
        {
            _contextFactory = contextFactory;
            _queryDispatcher = queryDispatcher;
        }

        public async Task<PageAdminModel> RetrieveAsync(GetForAdmin query)
        {
            using (var context = _contextFactory.Create())
            {
                var page = await context.Pages
                    .Include(x => x.PageLocalisations)
                    .Include(x => x.PagePermissions)
                    .FirstOrDefaultAsync(x => x.SiteId == query.SiteId && x.Id == query.Id && x.Status != PageStatus.Deleted);

                if (page == null)
                    return null;

                var result = new PageAdminModel
                {
                    Id = page.Id,
                    Name = page.Name,
                    Status = page.Status,
                    Url = page.Url,
                    Title = page.Title,
                    MetaDescription = page.MetaDescription,
                    MetaKeywords = page.MetaKeywords
                };

                var languages = await context.Languages
                    .Where(x => x.SiteId == query.SiteId && x.Status != LanguageStatus.Deleted)
                    .OrderBy(x => x.SortOrder)
                    .ToListAsync();

                foreach (var language in languages)
                {
                    var url = string.Empty;
                    var title = string.Empty;
                    var metaDescription = string.Empty;
                    var metaKeywords = string.Empty;

                    var existingLocalisation = page
                        .PageLocalisations
                        .FirstOrDefault(x => x.LanguageId == language.Id);

                    if (existingLocalisation != null)
                    {
                        url = existingLocalisation.Url;
                        title = existingLocalisation.Title;
                        metaDescription = existingLocalisation.MetaDescription;
                        metaKeywords = existingLocalisation.MetaKeywords;
                    }

                    result.PageLocalisations.Add(new PageLocalisationAdminModel
                    {
                        PageId = page.Id,
                        LanguageId = language.Id,
                        LanguageName = language.Name,
                        LanguageStatus = language.Status,
                        Url = url,
                        Title = title,
                        MetaDescription = metaDescription,
                        MetaKeywords = metaKeywords
                    });
                }

                foreach (var role in await _queryDispatcher.GetResultAsync<GetAllRoles, IEnumerable<ApplicationRole>>(new GetAllRoles()))
                {
                    var pagePermission = new PagePermissionModel
                    {
                        RoleId = role.Id,
                        RoleName = role.Name,
                        Disabled = role.Name == Administrator.Name
                    };

                    foreach (PermissionType permisisonType in Enum.GetValues(typeof(PermissionType)))
                    {
                        bool selected = page.PagePermissions
                            .FirstOrDefault(x => x.RoleId == role.Id && x.Type == permisisonType) != null;

                        pagePermission.PagePermissionTypes.Add(new PagePermissionTypeModel
                        {
                            Type = permisisonType,
                            Selected = selected || role.Name == Administrator.Name
                        });
                    }

                    result.PagePermissions.Add(pagePermission);
                }

                return result;
            }
        }
    }
}
