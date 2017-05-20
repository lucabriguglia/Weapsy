using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Weapsy.Domain.Apps;
using Weapsy.Domain.Modules;
using Weapsy.Domain.ModuleTypes;
using Weapsy.Domain.Pages;
using Weapsy.Domain.Sites;
using Weapsy.Framework.Caching;
using Weapsy.Framework.Queries;
using Weapsy.Reporting.Pages;
using Weapsy.Reporting.Pages.Queries;
using Page = Weapsy.Data.Entities.Page;
using PageModule = Weapsy.Data.Entities.PageModule;
using Weapsy.Reporting.Roles.Queries;

namespace Weapsy.Data.Reporting.Pages
{
    public class GetPageInfoHandler : IQueryHandlerAsync<GetPageInfo, PageInfo>
    {
        private readonly IContextFactory _contextFactory;
        private readonly ICacheManager _cacheManager;
        private readonly IQueryDispatcher _queryDispatcher;

        public GetPageInfoHandler(IContextFactory contextFactory,
            ICacheManager cacheManager,
            IQueryDispatcher queryDispatcher)
        {
            _contextFactory = contextFactory;
            _cacheManager = cacheManager;
            _queryDispatcher = queryDispatcher;
        }

        //needs refactoring (using factories)
        public async Task<PageInfo> RetrieveAsync(GetPageInfo query)
        {
            return await _cacheManager.Get(string.Format(CacheKeys.PageInfoCacheKey, query.SiteId, query.PageId, query.LanguageId), async () =>
            {
                using (var context = _contextFactory.Create())
                {
                    var page = await GetPage(context, query.SiteId, query.PageId);

                    if (page == null)
                        return null;

                    var roles = await GetRoles(page);

                    var site = await context.Sites.FirstOrDefaultAsync(x => x.Id == query.SiteId && x.Status == SiteStatus.Active);

                    if (site == null)
                        return null;

                    var url = page.Url;
                    var title = !string.IsNullOrWhiteSpace(page.Title) ? page.Title : site.Title;
                    var metaDescription = !string.IsNullOrWhiteSpace(page.MetaDescription) ? page.MetaDescription : site.MetaDescription;
                    var metaKeywords = !string.IsNullOrWhiteSpace(page.MetaKeywords) ? page.MetaKeywords : site.MetaKeywords;

                    if (query.LanguageId != Guid.Empty)
                    {
                        var pageLocalisation = page.PageLocalisations.FirstOrDefault(x => x.LanguageId == query.LanguageId);

                        if (pageLocalisation != null)
                        {
                            url = !string.IsNullOrWhiteSpace(pageLocalisation.Url) ? pageLocalisation.Url : url;
                            title = !string.IsNullOrWhiteSpace(pageLocalisation.Title) ? pageLocalisation.Title : title;
                            metaDescription = !string.IsNullOrWhiteSpace(pageLocalisation.MetaDescription) ? pageLocalisation.MetaDescription : metaDescription;
                            metaKeywords = !string.IsNullOrWhiteSpace(pageLocalisation.MetaKeywords) ? pageLocalisation.MetaKeywords : metaKeywords;
                        }
                    }

                    var result = new PageInfo
                    {
                        Page = new PageModel
                        {
                            Id = page.Id,
                            Name = page.Name,
                            Url = url,
                            Title = title,
                            MetaDescription = metaDescription,
                            MetaKeywords = metaKeywords,
                            Theme = "Default",
                            Template = "Default",
                            Roles = roles
                        },
                        Zones = await CreateZones(context, page, roles, query.LanguageId)
                    };

                    return result;
                }
            });
        }

        private async Task<Dictionary<PermissionType, IEnumerable<string>>> GetRoles(Entities.Page page)
        {
            var roles = new Dictionary<PermissionType, IEnumerable<string>>();
            foreach (PermissionType permisisonType in Enum.GetValues(typeof(PermissionType)))
            {
                var pageRoleIds = page.PagePermissions.Where(x => x.Type == permisisonType).Select(x => x.RoleId);
                var pageRoleNames = await _queryDispatcher
                    .DispatchAsync<GetRoleNamesFromRoleIds, IEnumerable<string>>(new GetRoleNamesFromRoleIds { RoleIds = pageRoleIds });
                roles.Add(permisisonType, pageRoleNames);
            }
            return roles;
        }

        private async Task<ICollection<ZoneModel>> CreateZones(WeapsyDbContext context, Page page, Dictionary<PermissionType, IEnumerable<string>> roles, Guid languageId)
        {
            var result = new List<ZoneModel>();

            var zones = page.PageModules.Where(x => x.Status == PageModuleStatus.Active).GroupBy(x => x.Zone);

            foreach (var zone in zones)
            {
                var zoneModel = CreateZone(context, zone, roles, languageId);
                result.Add(await zoneModel);
            }

            return result;
        }

        private async Task<ZoneModel> CreateZone(WeapsyDbContext context, IGrouping<string, PageModule> zone, Dictionary<PermissionType, IEnumerable<string>> roles, Guid languageId)
        {
            var result = new ZoneModel
            {
                Name = zone.Key
            };

            foreach (var pageModule in zone.OrderBy(x => x.SortOrder))
            {
                var moduleModel = await CreateModule(context, pageModule, roles, languageId);

                if (moduleModel == null)
                    continue;

                result.Modules.Add(moduleModel);
            }

            return result;
        }

        private async Task<ModuleModel> CreateModule(WeapsyDbContext context, PageModule pageModule, Dictionary<PermissionType, IEnumerable<string>> roles, Guid languageId)
        {
            var module = await context.Modules
                .Include(x => x.ModuleType).ThenInclude(x => x.App)
                .FirstOrDefaultAsync(x => x.Id == pageModule.ModuleId && x.Status != ModuleStatus.Deleted);

            if (module == null)
                return null;

            var moduleType = module.ModuleType;

            if (moduleType == null || moduleType.Status == ModuleTypeStatus.Deleted)
                return null;

            var app = moduleType.App;

            if (app == null || app.Status == AppStatus.Deleted)
                return null;

            var moduleRoles = new Dictionary<PermissionType, IEnumerable<string>>();

            if (pageModule.InheritPermissions)
            {
                moduleRoles = roles;
            }
            else
            {
                foreach (PermissionType permisisonType in Enum.GetValues(typeof(PermissionType)))
                {
                    var pageRoleIds = pageModule.PageModulePermissions.Where(x => x.Type == permisisonType).Select(x => x.RoleId);
                    var pageRoles = await _queryDispatcher
                            .DispatchAsync<GetRoleNamesFromRoleIds, IEnumerable<string>>(new GetRoleNamesFromRoleIds { RoleIds = pageRoleIds });
                    moduleRoles.Add(permisisonType, pageRoles);
                }
            }

            var title = pageModule.Title;

            if (languageId != Guid.Empty)
            {
                var pageModuleLocalisation = pageModule.PageModuleLocalisations.FirstOrDefault(x => x.LanguageId == languageId);

                if (pageModuleLocalisation != null)
                {
                    title = !string.IsNullOrWhiteSpace(pageModuleLocalisation.Title)
                        ? pageModuleLocalisation.Title
                        : title;
                }
            }

            var moduleModel = new ModuleModel
            {
                Id = pageModule.ModuleId,
                PageModuleId = pageModule.Id,
                Title = title,
                Zone = pageModule.Zone,
                SortOrder = pageModule.SortOrder,
                Template = "Default",
                Roles = moduleRoles,
                ModuleType = new ModuleTypeModel
                {
                    ViewType = moduleType.ViewType,
                    ViewName = moduleType.ViewName,
                    EditType = moduleType.EditType,
                    EditUrl = $"{app.Folder}/{moduleType.EditUrl}"
                }
            };

            return moduleModel;
        }

        private async Task<Page> GetPage(WeapsyDbContext context, Guid siteId, Guid pageId)
        {
            var page = await context.Pages
                .Include(x => x.PageLocalisations)
                .Include(x => x.PagePermissions)
                .FirstOrDefaultAsync(x => x.SiteId == siteId && x.Id == pageId && x.Status == PageStatus.Active);

            if (page == null)
                return null;

            page.PageModules = await context.PageModules
                .Include(y => y.PageModuleLocalisations)
                .Include(y => y.PageModulePermissions)
                .Where(x => x.PageId == pageId && x.Status == PageModuleStatus.Active)
                .ToListAsync();

            return page;
        }
    }
}
