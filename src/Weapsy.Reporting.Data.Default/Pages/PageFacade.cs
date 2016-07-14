using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Weapsy.Core.Caching;
using Weapsy.Domain.Model.Languages;
using Weapsy.Domain.Model.Modules;
using Weapsy.Domain.Model.ModuleTypes;
using Weapsy.Domain.Model.Pages;
using Weapsy.Reporting.Pages;
using Microsoft.AspNetCore.Identity;
using Weapsy.Core.Identity;

namespace Weapsy.Reporting.Data.Default.Pages
{
    public class PageFacade : IPageFacade
    {
        private readonly IPageRepository _pageRepository;
        private readonly ILanguageRepository _languageRepository;
        private readonly IModuleRepository _moduleRepository;
        private readonly IModuleTypeRepository _moduleTypeRepository;
        private readonly ICacheManager _cacheManager;
        private readonly IMapper _mapper;
        private readonly RoleManager<IdentityRole> _roleManager;

        public PageFacade(IPageRepository pageRepository, 
            ILanguageRepository languageRepository,
            IModuleRepository moduleRepository,
            IModuleTypeRepository moduleTypeRepository,
            ICacheManager cacheManager, 
            IMapper mapper,
            RoleManager<IdentityRole> roleManager)
        {
            _pageRepository = pageRepository;
            _languageRepository = languageRepository;
            _moduleRepository = moduleRepository;
            _moduleTypeRepository = moduleTypeRepository;
            _cacheManager = cacheManager;
            _mapper = mapper;
            _roleManager = roleManager;
        }

        public PageViewModel GetPageViewModel(Guid siteId, Guid pageId, Guid languageId = new Guid())
        {
            return _cacheManager.Get(string.Format(CacheKeys.PageCacheKey, siteId, pageId), () =>
            {
                var page = _pageRepository.GetById(siteId, pageId);

                if (page == null || page.Status != PageStatus.Active)
                    return null;

                var result = new PageViewModel();

                result.Page = new PageModel
                {
                    Id = page.Id,
                    Name = page.Name,
                    Url = page.Url,
                    Title = page.Title,
                    MetaDescription = page.MetaDescription,
                    MetaKeywords = page.MetaKeywords/*,
                    ViewRoles = GetViewRoles(page)*/
                };

                result.Page.Template = new PageTemplateModel
                {
                    ViewName = "Default"
                };

                result.Zones = GetZones(page);

                return result;
            });
        }

        private IList<string> GetViewRoles(Page page)
        {
            var roleIds = page.Permissions.Where(x => x.Type == PermissionType.View).Select(x => x.RoleId);

            var result = new List<string>();

            foreach (var roleId in roleIds)
            {
                if (Enum.IsDefined(typeof(Roles), roleId))
                {
                    result.Add(Enum.GetName(typeof(Roles), roleId));
                    continue;
                }

                var role = _roleManager.FindByIdAsync(roleId).Result;
                if (role != null)
                    result.Add(role.Name);
            }

            return result;
        }

        private ICollection<ZoneModel> GetZones(Page page)
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
                    var moduleModel = BuildModuleModel(pageModule);

                    if (moduleModel == null)
                        continue;

                    zoneModel.Modules.Add(moduleModel);
                }

                result.Add(zoneModel);
            }

            return result;
        }

        private ModuleModel BuildModuleModel(PageModule pageModule)
        {
            var module = _moduleRepository.GetById(pageModule.ModuleId);

            if (module == null)
                return null;

            var moduleType = _moduleTypeRepository.GetById(module.ModuleTypeId);

            if (moduleType == null)
                return null;

            var moduleModel = new ModuleModel
            {
                Id = pageModule.ModuleId,
                Title = pageModule.Title,
                SortOrder = pageModule.SortOrder
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

        public PageViewModel GetPageViewModel(Guid siteId, string name, Guid languageId = new Guid())
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<PageAdminListModel>> GetAllForAdminAsync(Guid siteId)
        {
            var pages = _pageRepository.GetAll(siteId).Where(x => x.Status != PageStatus.Deleted);
            return _mapper.Map<IEnumerable<PageAdminListModel>>(pages);
        }

        public async Task<PageAdminModel> GetAdminModelAsync(Guid siteId, Guid pageId)
        {
            var page = _pageRepository.GetById(siteId, pageId);

            if (page == null || page.Status == PageStatus.Deleted)
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

            var languages = _languageRepository.GetAll(siteId).Where(x => x.Status != LanguageStatus.Deleted);

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
                    Url = url,
                    Title = title,
                    MetaDescription = metaDescription,
                    MetaKeywords = metaKeywords
                });
            }

            return result;
        }

        public async Task<PageAdminModel> GetDefaultAdminModelAsync(Guid siteId)
        {
            var result = new PageAdminModel();

            var languages = _languageRepository.GetAll(siteId).Where(x => x.Status != LanguageStatus.Deleted);

            foreach (var language in languages)
            {
                result.PageLocalisations.Add(new PageLocalisationAdminModel
                {
                    LanguageId = language.Id,
                    LanguageName = language.Name
                });
            }

            return result;
        }
    }
}