using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Weapsy.Domain.Languages;
using Weapsy.Domain.Modules;
using Weapsy.Domain.ModuleTypes;
using Weapsy.Domain.Pages;
using Weapsy.Reporting.Pages;
using Microsoft.AspNetCore.Identity;
using Weapsy.Services.Identity;

namespace Weapsy.Reporting.Data.Default.Pages
{
    public class PageInfoFactory : IPageInfoFactory
    {
        private readonly IPageRepository _pageRepository;
        private readonly ILanguageRepository _languageRepository;
        private readonly IModuleRepository _moduleRepository;
        private readonly IModuleTypeRepository _moduleTypeRepository;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IRoleService _roleService;

        public PageInfoFactory(IPageRepository pageRepository,
            ILanguageRepository languageRepository,
            IModuleRepository moduleRepository,
            IModuleTypeRepository moduleTypeRepository,
            RoleManager<IdentityRole> roleManager,
            IRoleService roleService)
        {
            _pageRepository = pageRepository;
            _languageRepository = languageRepository;
            _moduleRepository = moduleRepository;
            _moduleTypeRepository = moduleTypeRepository;
            _roleManager = roleManager;
            _roleService = roleService;
        }

        // needs refactoring
        public PageInfo CreatePageInfo(Guid siteId, Guid pageId, Guid languageId = new Guid())
        {
            var page = _pageRepository.GetById(siteId, pageId);

            if (page == null || page.Status != PageStatus.Active)
                return null;

            var pageViewRoleIds = page.PagePermissions.Where(x => x.Type == PermissionType.View).Select(x => x.RoleId);
            var pageViewRoles = _roleService.GetRolesFromIds(pageViewRoleIds);

            var url = page.Url;
            var title = page.Title;
            var metaDescription = page.MetaDescription;
            var metaKeywords = page.MetaKeywords;

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
                Id = page.Id,
                Name = page.Name,
                Url = url,
                Title = title,
                MetaDescription = metaDescription,
                MetaKeywords = metaKeywords,
                ViewRoles = pageViewRoles.Select(x => x.Name),
                Theme = new ThemeModel
                {
                    Name = "Default"
                },
                Template = new PageTemplateModel
                {
                    ViewName = "Default"
                },
                Zones = CreateZones(page, pageViewRoles, languageId)
            };

            return result;
        }

        private ICollection<ZoneModel> CreateZones(Page page, IEnumerable<IdentityRole> pageViewRoles, Guid languageId)
        {
            var result = new List<ZoneModel>();

            var zones = page.PageModules.Where(x => x.Status == PageModuleStatus.Active).GroupBy(x => x.Zone);

            foreach (var zone in zones)
            {
                var zoneModel = CreateZone(zone, pageViewRoles, languageId);
                result.Add(zoneModel);
            }

            return result;
        }

        private ZoneModel CreateZone(IGrouping<string, PageModule> zone, IEnumerable<IdentityRole> pageViewRoles, Guid languageId)
        {
            var result = new ZoneModel
            {
                Name = zone.Key
            };

            foreach (var pageModule in zone.OrderBy(x => x.SortOrder))
            {
                var moduleModel = CreateModule(pageModule, pageViewRoles, languageId);

                if (moduleModel == null)
                    continue;

                result.Modules.Add(moduleModel);
            }

            return result;
        }

        private ModuleModel CreateModule(PageModule pageModule, IEnumerable<IdentityRole> pageViewRoles, Guid languageId)
        {
            var module = _moduleRepository.GetById(pageModule.ModuleId);

            if (module == null)
                return null;

            var moduleType = _moduleTypeRepository.GetById(module.ModuleTypeId);

            if (moduleType == null)
                return null;

            IEnumerable<string> moduleViewRoleNames;

            if (pageModule.InheritPermissions)
            {
                moduleViewRoleNames = pageViewRoles.Select(x => x.Name);
            }
            else
            {
                var moduleViewRoleIds = pageModule.PageModulePermissions.Where(x => x.Type == PermissionType.View).Select(x => x.RoleId);
                moduleViewRoleNames = _roleService.GetRolesFromIds(moduleViewRoleIds).Select(x => x.Name);
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
                ViewRoles = moduleViewRoleNames,
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
    }
}