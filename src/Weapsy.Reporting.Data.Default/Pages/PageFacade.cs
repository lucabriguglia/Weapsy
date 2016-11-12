using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Weapsy.Infrastructure.Caching;
using Weapsy.Domain.Languages;
using Weapsy.Domain.Pages;
using Weapsy.Reporting.Pages;
using Weapsy.Services.Identity;

namespace Weapsy.Reporting.Data.Default.Pages
{
    public class PageFacade : IPageFacade
    {
        private readonly IPageRepository _pageRepository;
        private readonly ILanguageRepository _languageRepository;
        private readonly ICacheManager _cacheManager;
        private readonly IMapper _mapper;
        private readonly IRoleService _roleService;
        private readonly IPageInfoFactory _pageViewFactory;
        private readonly IPageAdminFactory _pageAdminFactory;

        public PageFacade(IPageRepository pageRepository, 
            ILanguageRepository languageRepository,
            ICacheManager cacheManager, 
            IMapper mapper,
            IRoleService roleService,
            IPageInfoFactory pageViewFactory,
            IPageAdminFactory pageAdminFactory)
        {
            _pageRepository = pageRepository;
            _languageRepository = languageRepository;
            _cacheManager = cacheManager;
            _mapper = mapper;
            _roleService = roleService;
            _pageViewFactory = pageViewFactory;
            _pageAdminFactory = pageAdminFactory;
        }

        public PageInfo GetPageInfo(Guid siteId, Guid pageId, Guid languageId = new Guid())
        {
            return _cacheManager.Get(string.Format(CacheKeys.PageInfoCacheKey, siteId, pageId, languageId), () =>
            {
                return _pageViewFactory.CreatePageInfo(siteId, pageId, languageId);
            });
        }

        public PageInfo GetPageInfo(Guid siteId, string name, Guid languageId = new Guid())
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PageAdminListModel> GetAllForAdmin(Guid siteId)
        {
            var pages = _pageRepository.GetAll(siteId);
            return _mapper.Map<IEnumerable<PageAdminListModel>>(pages);
        }

        public PageAdminModel GetAdminModel(Guid siteId, Guid pageId)
        {
            return _pageAdminFactory.GetAdminModel(siteId, pageId);
        }

        public PageAdminModel GetDefaultAdminModel(Guid siteId)
        {
            return _pageAdminFactory.GetDefaultAdminModel(siteId);
        }

        public PageModuleAdminModel GetModuleAdminModel(Guid siteId, Guid pageId, Guid pageModuleId)
        {
            var page = _pageRepository.GetById(siteId, pageId);

            if (page == null)
                return null;

            var pageModule = page.PageModules.FirstOrDefault(x => x.Id == pageModuleId);

            if (pageModule == null)
                return null;

            var result = new PageModuleAdminModel
            {
                PageId = page.Id,
                ModuleId = pageModule.ModuleId,
                PageModuleId = pageModule.Id,
                Title = pageModule.Title,
                InheritPermissions = pageModule.InheritPermissions
            };

            var languages = _languageRepository.GetAll(siteId);

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