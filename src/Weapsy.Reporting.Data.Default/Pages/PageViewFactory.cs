using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Weapsy.Domain.Model.Languages;
using Weapsy.Domain.Model.Modules;
using Weapsy.Domain.Model.ModuleTypes;
using Weapsy.Domain.Model.Pages;
using Weapsy.Reporting.Pages;
using Microsoft.AspNetCore.Identity;
using Weapsy.Core.Identity;
using Weapsy.Services.Identity;

namespace Weapsy.Reporting.Data.Default.Pages
{
    public class PageViewFactory : IPageViewFactory
    {
        private readonly IPageRepository _pageRepository;
        private readonly ILanguageRepository _languageRepository;
        private readonly IModuleRepository _moduleRepository;
        private readonly IModuleTypeRepository _moduleTypeRepository;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IRoleService _roleService;

        public PageViewFactory(IPageRepository pageRepository,
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
        public PageInfo GetPageInfo(Guid siteId, Guid pageId, Guid languageId = new Guid())
        {
            var page = _pageRepository.GetById(siteId, pageId);

            if (page == null || page.Status != PageStatus.Active)
                return null;

            var result = new PageInfo();

            var pageViewRoleIds = page.PagePermissions.Where(x => x.Type == PermissionType.View).Select(x => x.RoleId);
            var pageViewRoleNames = GetRoleNames(pageViewRoleIds);

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

            result.Page = new PageModel
            {
                Id = page.Id,
                Name = page.Name,
                Url = url,
                Title = title,
                MetaDescription = metaDescription,
                MetaKeywords = metaKeywords,
                ViewRoles = pageViewRoleNames
            };

            result.Page.Template = new PageTemplateModel
            {
                ViewName = "Default"
            };

            result.Zones = GetZones(page, pageViewRoleNames, languageId);

            return result;
        }

        private IList<string> GetRoleNames(IEnumerable<string> roleIds)
        {
            var result = new List<string>();

            foreach (var roleId in roleIds)
            {
                int id;

                if (int.TryParse(roleId, out id))
                {
                    if (Enum.IsDefined(typeof(Roles), id))
                    {
                        result.Add(Enum.GetName(typeof(Roles), id));
                        continue;
                    }
                }

                var role = _roleManager.FindByIdAsync(roleId).Result;
                if (role != null)
                    result.Add(role.Name);
            }

            return result;
        }

        private ICollection<ZoneModel> GetZones(Page page, IEnumerable<string> pageViewRoleNames, Guid languageId)
        {
            var result = new List<ZoneModel>();

            var zones = page.PageModules.Where(x => x.Status == PageModuleStatus.Active).GroupBy(x => x.Zone);

            foreach (var zone in zones)
            {
                var zoneModel = new ZoneModel
                {
                    Name = zone.Key
                };

                foreach (var pageModule in zone.OrderBy(x => x.SortOrder))
                {
                    var moduleModel = BuildModuleModel(pageModule, pageViewRoleNames, languageId);

                    if (moduleModel == null)
                        continue;

                    zoneModel.Modules.Add(moduleModel);
                }

                result.Add(zoneModel);
            }

            return result;
        }

        private ModuleModel BuildModuleModel(PageModule pageModule, IEnumerable<string> pageViewRoleNames, Guid languageId)
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
                moduleViewRoleNames = pageViewRoleNames;
            }
            else
            {
                var moduleViewRoleIds = pageModule.PageModulePermissions.Where(x => x.Type == PermissionType.View).Select(x => x.RoleId);
                moduleViewRoleNames = GetRoleNames(moduleViewRoleIds);
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
                SortOrder = pageModule.SortOrder,
                ViewRoles = moduleViewRoleNames
            };

            moduleModel.ModuleType = new ModuleTypeModel
            {
                ViewType = moduleType.ViewType,
                ViewName = moduleType.ViewName,
                EditType = moduleType.EditType,
                EditUrl = moduleType.EditUrl
            };

            moduleModel.Template = new ModuleTemplateModel
            {
                ViewName = "Default"
            };

            return moduleModel;
        }
    }
}