using System;
using System.Linq;
using Weapsy.Domain.Languages;
using Weapsy.Domain.Pages;
using Weapsy.Infrastructure.Identity;
using Weapsy.Reporting.Pages;
using Weapsy.Services.Identity;

namespace Weapsy.Reporting.Data.Pages
{
    public class PageAdminFactory : IPageAdminFactory
    {
        private readonly IPageRepository _pageRepository;
        private readonly ILanguageRepository _languageRepository;
        private readonly IRoleService _roleService;

        public PageAdminFactory(IPageRepository pageRepository,
            ILanguageRepository languageRepository,
            IRoleService roleService)
        {
            _pageRepository = pageRepository;
            _languageRepository = languageRepository;
            _roleService = roleService;
        }

        public PageAdminModel GetAdminModel(Guid siteId, Guid pageId)
        {
            var page = _pageRepository.GetById(siteId, pageId);

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

            var languages = _languageRepository.GetAll(siteId);

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
                    bool selected = page.PagePermissions
                        .FirstOrDefault(x => x.RoleId == role.Id && x.Type == permisisonType) != null;

                    pagePermission.PagePermissionTypes.Add(new PagePermissionTypeModel
                    {
                        Type = permisisonType,
                        Selected = selected || role.Name == DefaultRoleNames.Administrator
                    });
                }

                result.PagePermissions.Add(pagePermission);
            }

            return result;
        }

        public PageAdminModel GetDefaultAdminModel(Guid siteId)
        {
            var result = new PageAdminModel();

            var languages = _languageRepository.GetAll(siteId);

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

            return result;
        }
    }
}