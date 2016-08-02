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
using Weapsy.Services.Identity;

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
        private readonly IRoleService _roleService;
        private readonly IPageViewFactory _pageViewFactory;
        private readonly IPageAdminFactory _pageAdminFactory;

        public PageFacade(IPageRepository pageRepository, 
            ILanguageRepository languageRepository,
            IModuleRepository moduleRepository,
            IModuleTypeRepository moduleTypeRepository,
            ICacheManager cacheManager, 
            IMapper mapper,
            RoleManager<IdentityRole> roleManager,
            IRoleService roleService,
            IPageViewFactory pageViewFactory,
            IPageAdminFactory pageAdminFactory)
        {
            _pageRepository = pageRepository;
            _languageRepository = languageRepository;
            _moduleRepository = moduleRepository;
            _moduleTypeRepository = moduleTypeRepository;
            _cacheManager = cacheManager;
            _mapper = mapper;
            _roleManager = roleManager;
            _roleService = roleService;
            _pageViewFactory = pageViewFactory;
            _pageAdminFactory = pageAdminFactory;
        }

        public PageViewModel GetPageViewModel(Guid siteId, Guid pageId, Guid languageId = new Guid())
        {
            return _cacheManager.Get(string.Format(CacheKeys.PageCacheKey, siteId, pageId, languageId), () =>
            {
                return _pageViewFactory.GetPageViewModel(siteId, pageId, languageId);
            });
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
            return _pageAdminFactory.GetAdminModel(siteId, pageId);
        }

        public async Task<PageAdminModel> GetDefaultAdminModelAsync(Guid siteId)
        {
            return _pageAdminFactory.GetDefaultAdminModel(siteId);
        }

        public async Task<PageModuleAdminModel> GetModuleAdminModelAsync(Guid siteId, Guid pageId, Guid pageModuleId)
        {
            var page = _pageRepository.GetById(siteId, pageId);

            if (page == null || page.Status == PageStatus.Deleted)
                return null;

            var pageModule = page.PageModules.FirstOrDefault(x => x.Id == pageModuleId);

            if (pageModule == null || pageModule.Status == PageModuleStatus.Deleted)
                return null;

            var result = new PageModuleAdminModel
            {
                PageId = page.Id,
                ModuleId = pageModule.ModuleId,
                PageModuleId = pageModule.Id,
                Title = pageModule.Title,
                InheritPermissions = pageModule.InheritPermissions
            };

            var languages = _languageRepository.GetAll(siteId).Where(x => x.Status != LanguageStatus.Deleted);

            foreach (var language in languages)
            {
                var title = string.Empty;

                var existingLocalisation = pageModule
                    .PageModuleLocalisations
                    .FirstOrDefault(x => x.LanguageId == language.Id);

                if (existingLocalisation != null)
                {
                    title = existingLocalisation.Title;
                }

                result.PageModuleLocalisations.Add(new PageModuleLocalisationAdminModel
                {
                    PageModuleId = pageModule.Id,
                    LanguageId = language.Id,
                    LanguageName = language.Name,
                    Title = title
                });
            }

            foreach (var role in _roleService.GetAllRoles())
            {
                bool selected = pageModule.PageModulePermissions.FirstOrDefault(x => x.RoleId == role.Id) != null;

                result.PageModulePermissions.Add(new PageModulePermissionModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name,
                    Type = PermissionType.View,
                    Selected = selected
                });
            }

            return result;
        }
    }
}