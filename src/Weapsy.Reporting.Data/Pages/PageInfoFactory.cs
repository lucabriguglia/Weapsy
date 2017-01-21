using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Weapsy.Data;
using Weapsy.Domain.Modules;
using Weapsy.Domain.ModuleTypes;
using Weapsy.Domain.Pages;
using Weapsy.Domain.Sites;
using Weapsy.Reporting.Pages;
using Weapsy.Services.Identity;
using Page = Weapsy.Data.Entities.Page;
using PageModule = Weapsy.Data.Entities.PageModule;

namespace Weapsy.Reporting.Data.Pages
{
    public class PageInfoFactory : IPageInfoFactory
    {
        private readonly IDbContextFactory _dbContextFactory;
        private readonly IRoleService _roleService;

        public PageInfoFactory(IDbContextFactory dbContextFactory,
            IRoleService roleService)
        {
            _dbContextFactory = dbContextFactory;
            _roleService = roleService;
        }

        // needs refactoring
        public PageInfo CreatePageInfo(Guid siteId, Guid pageId, Guid languageId = new Guid())
        {
            using (var context = _dbContextFactory.Create())
            {
                var page = GetPage(context, siteId, pageId);

                if (page == null)
                    return null;

                var roles = new Dictionary<PermissionType, IEnumerable<string>>();
                foreach (PermissionType permisisonType in Enum.GetValues(typeof(PermissionType)))
                {
                    var pageRoleIds = page.PagePermissions.Where(x => x.Type == permisisonType).Select(x => x.RoleId);
                    var pageRoles = _roleService.GetRolesFromIds(pageRoleIds);
                    roles.Add(permisisonType, pageRoles.Select(x => x.Name));
                }

                var site = context.Sites.FirstOrDefault(x => x.Id == siteId && x.Status == SiteStatus.Active);

                if (site == null)
                    return null;

                var url = page.Url;
                var title = !string.IsNullOrWhiteSpace(page.Title) ? page.Title : site.Title;
                var metaDescription = !string.IsNullOrWhiteSpace(page.MetaDescription) ? page.MetaDescription : site.MetaDescription;
                var metaKeywords = !string.IsNullOrWhiteSpace(page.MetaKeywords) ? page.MetaKeywords : site.MetaKeywords;

                if (languageId != Guid.Empty)
                {
                    var pageLocalisation = page.PageLocalisations.FirstOrDefault(x => x.LanguageId == languageId);

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
                        Roles = roles
                    },
                    Theme = new ThemeModel
                    {
                        Name = "Default"
                    },
                    Template = new PageTemplateModel
                    {
                        ViewName = "Default"
                    },
                    Zones = CreateZones(context, page, roles, languageId)
                };

                return result;
            }
        }

        private ICollection<ZoneModel> CreateZones(WeapsyDbContext context, Page page, Dictionary<PermissionType, IEnumerable<string>> roles, Guid languageId)
        {
            var result = new List<ZoneModel>();

            var zones = page.PageModules.Where(x => x.Status == PageModuleStatus.Active).GroupBy(x => x.Zone);

            foreach (var zone in zones)
            {
                var zoneModel = CreateZone(context, zone, roles, languageId);
                result.Add(zoneModel);
            }

            return result;
        }

        private ZoneModel CreateZone(WeapsyDbContext context, IGrouping<string, PageModule> zone, Dictionary<PermissionType, IEnumerable<string>> roles, Guid languageId)
        {
            var result = new ZoneModel
            {
                Name = zone.Key
            };

            foreach (var pageModule in zone.OrderBy(x => x.SortOrder))
            {
                var moduleModel = CreateModule(context, pageModule, roles, languageId);

                if (moduleModel == null)
                    continue;

                result.Modules.Add(moduleModel);
            }

            return result;
        }

        private ModuleModel CreateModule(WeapsyDbContext context, PageModule pageModule, Dictionary<PermissionType, IEnumerable<string>> roles, Guid languageId)
        {
            var module = context.Modules.FirstOrDefault(x => x.Id == pageModule.ModuleId && x.Status != ModuleStatus.Deleted);

            if (module == null)
                return null;

            var moduleType = context.ModuleTypes.FirstOrDefault(x => x.Id == module.ModuleTypeId && x.Status != ModuleTypeStatus.Deleted);

            if (moduleType == null)
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
                    var pageRoles = _roleService.GetRolesFromIds(pageRoleIds);
                    moduleRoles.Add(permisisonType, pageRoles.Select(x => x.Name));
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
                Roles = moduleRoles,
                ModuleType = new ModuleTypeModel
                {
                    ViewType = moduleType.ViewType,
                    ViewName = moduleType.ViewName,
                    EditType = moduleType.EditType,
                    EditUrl = moduleType.EditUrl
                },
                Template = new ModuleTemplateModel
                {
                    ViewName = "Default"
                }
            };

            return moduleModel;
        }

        private Page GetPage(WeapsyDbContext context, Guid siteId, Guid pageId)
        {
            var page = context.Pages
                .Include(x => x.PageLocalisations)
                .Include(x => x.PagePermissions)
                .FirstOrDefault(x => x.SiteId == siteId && x.Id == pageId && x.Status == PageStatus.Active);

            if (page == null)
                return null;

            page.PageModules = context.PageModules
                .Include(y => y.PageModuleLocalisations)
                .Include(y => y.PageModulePermissions)
                .Where(x => x.PageId == pageId && x.Status == PageModuleStatus.Active)
                .ToList();

            return page;
        }
    }
}