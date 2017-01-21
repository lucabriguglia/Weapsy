using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Weapsy.Data;
using Weapsy.Domain.Pages;
using Weapsy.Infrastructure.Caching;
using Weapsy.Reporting.Pages;
using Weapsy.Services.Identity;
using Microsoft.EntityFrameworkCore;
using Weapsy.Domain.Languages;
using Weapsy.Infrastructure.Identity;
using Page = Weapsy.Data.Entities.Page;

namespace Weapsy.Reporting.Data.Pages
{
    public class PageFacade : IPageFacade
    {
        private readonly IDbContextFactory _dbContextFactory;
        private readonly ICacheManager _cacheManager;
        private readonly IMapper _mapper;
        private readonly IRoleService _roleService;
        private readonly IPageInfoFactory _pageViewFactory;
        private readonly IPageAdminFactory _pageAdminFactory;

        public PageFacade(IDbContextFactory dbContextFactory,
            ICacheManager cacheManager,
            IMapper mapper,
            IRoleService roleService,
            IPageInfoFactory pageViewFactory,
            IPageAdminFactory pageAdminFactory)
        {
            _dbContextFactory = dbContextFactory;
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
            using (var context = _dbContextFactory.Create())
            {
                var pages = context.Pages
                    .Include(x => x.PageLocalisations)
                    .Where(x => x.SiteId == siteId && x.Status != PageStatus.Deleted)
                    .OrderBy(x => x.Name).ToList();

                return _mapper.Map<IEnumerable<PageAdminListModel>>(pages);
            }            
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
            using (var context = _dbContextFactory.Create())
            {
                var page = GetPage(context, siteId, pageId);

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

                var languages = context.Languages
                    .Where(x => x.SiteId == siteId && x.Status != LanguageStatus.Deleted)
                    .OrderBy(x => x.SortOrder)
                    .ToList();

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
                        LanguageStatus = language.Status,
                        Title = title
                    });
                }

                foreach (var role in _roleService.GetAllRoles())
                {
                    var pageModulePermission = new PageModulePermissionModel
                    {
                        RoleId = role.Id,
                        RoleName = role.Name,
                        Disabled = role.Name == DefaultRoleNames.Administrator
                    };

                    foreach (PermissionType permisisonType in Enum.GetValues(typeof(PermissionType)))
                    {
                        bool selected = pageModule.PageModulePermissions
                            .FirstOrDefault(x => x.RoleId == role.Id && x.Type == permisisonType) != null;

                        pageModulePermission.PageModulePermissionTypes.Add(new PageModulePermissionTypeModel
                        {
                            Type = permisisonType,
                            Selected = selected
                        });
                    }

                    result.PageModulePermissions.Add(pageModulePermission);
                }

                return result;
            }
        }

        public Guid? GetIdBySlug(Guid siteId, string slug)
        {
            using (var context = _dbContextFactory.Create())
            {
                var dbEntity = context.Pages
                    .FirstOrDefault(x => x.SiteId == siteId
                    && x.Status == PageStatus.Active
                    && x.Url == slug);

                return dbEntity?.Id;
            }
        }

        public Guid? GetIdBySlug(Guid siteId, string slug, Guid languageId)
        {
            using (var context = _dbContextFactory.Create())
            {
                var dbEntity = context.PageLocalisations
                    .FirstOrDefault(x => x.Page.SiteId == siteId
                    && x.Page.Status == PageStatus.Active
                    && x.Url == slug
                    && x.LanguageId == languageId);

                return dbEntity?.PageId;
            }
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