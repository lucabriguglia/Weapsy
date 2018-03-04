using FluentValidation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Weapsy.Cqrs;
using Weapsy.Domain.Languages;
using Weapsy.Domain.Languages.Commands;
using Weapsy.Domain.Menus;
using Weapsy.Domain.Menus.Commands;
using Weapsy.Domain.Modules;
using Weapsy.Domain.Modules.Commands;
using Weapsy.Domain.ModuleTypes;
using Weapsy.Domain.Pages;
using Weapsy.Domain.Pages.Commands;
using Weapsy.Domain.Sites;
using Weapsy.Domain.Sites.Commands;
using Weapsy.Domain.Roles.DefaultRoles;
using Weapsy.Reporting.Sites.Queries;

namespace Weapsy.Services.Installation
{
    // temporary implementation, it will be based on site templates
    public class SiteInstallationService : ISiteInstallationService
    {
        private readonly ISiteRepository _siteRepository;
        private readonly IValidator<CreateSite> _createSiteValidator;
        private readonly IValidator<UpdateSiteDetails> _updateSiteDetailsValidator;
        private readonly ILanguageRepository _languageRepository;
        private readonly IValidator<CreateLanguage> _createLanguageValidator;
        private readonly IValidator<ActivateLanguage> _activateLanguageValidator;
        private readonly ILanguageSortOrderGenerator _languageSortOrderGenerator;
        private readonly IPageRepository _pageRepository;
        private readonly IValidator<CreatePage> _createPageValidator;
        private readonly IValidator<ActivatePage> _activatePageValidator;
        private readonly IValidator<AddPageModule> _addPageModuleValidator;
        private readonly IModuleRepository _moduleRepository;
        private readonly IValidator<CreateModule> _createModuleValidator;
        private readonly IMenuRepository _menuRepository;
        private readonly IValidator<CreateMenu> _createMenuValidator;
        private readonly IValidator<AddMenuItem> _addMenuItemValidator;
        private readonly IModuleTypeRepository _moduleTypeRepository;
        private readonly IDispatcher _dispatcher;

        public SiteInstallationService(ISiteRepository siteRepository,
            IValidator<CreateSite> createSiteValidator,
            IValidator<UpdateSiteDetails> updateSiteDetailsValidator,
            ILanguageRepository languageRepository,
            IValidator<CreateLanguage> createLanguageValidator,
            IValidator<ActivateLanguage> activateLanguageValidator,
            ILanguageSortOrderGenerator languageSortOrderGenerator,
            IPageRepository pageRepository,
            IValidator<CreatePage> createPageValidator,
            IValidator<ActivatePage> activatePageValidator,
            IValidator<AddPageModule> addPageModuleValidator,
            IModuleRepository moduleRepository,
            IValidator<CreateModule> createModuleValidator,
            IMenuRepository menuRepository,
            IValidator<CreateMenu> createMenuValidator,
            IValidator<AddMenuItem> addMenuItemValidator,
            IModuleTypeRepository moduleTypeRepository, 
            IDispatcher dispatcher)
        {
            _siteRepository = siteRepository;
            _createSiteValidator = createSiteValidator;
            _updateSiteDetailsValidator = updateSiteDetailsValidator;
            _languageRepository = languageRepository;
            _createLanguageValidator = createLanguageValidator;
            _activateLanguageValidator = activateLanguageValidator;
            _languageSortOrderGenerator = languageSortOrderGenerator;
            _pageRepository = pageRepository;
            _createPageValidator = createPageValidator;
            _activatePageValidator = activatePageValidator;
            _addPageModuleValidator = addPageModuleValidator;
            _moduleRepository = moduleRepository;
            _createModuleValidator = createModuleValidator;
            _menuRepository = menuRepository;
            _createMenuValidator = createMenuValidator;
            _addMenuItemValidator = addMenuItemValidator;
            _moduleTypeRepository = moduleTypeRepository;
            _dispatcher = dispatcher;
        }

        public void VerifySiteInstallation()
        {
            if (_siteRepository.GetByName("Default") == null)
                InstallDefaultSite();
        }

        public void InstallDefaultSite()
        {
            var siteId = Guid.NewGuid();
            var englishLanguageId = Guid.NewGuid();
            var mainMenuId = Guid.NewGuid();
            var homePageId = Guid.NewGuid();

            // ===== Site ===== //

            var site = Site.CreateNew(new CreateSite
            {
                Id = siteId,
                Name = "Default"
            }, _createSiteValidator);

            _siteRepository.Create(site);

            // ===== Languages ===== //

            var language = Language.CreateNew(new CreateLanguage
            {
                SiteId = siteId,
                Id = englishLanguageId,
                Name = "English",
                CultureName = "en",
                Url = "en"
            }, _createLanguageValidator, _languageSortOrderGenerator);

            language.Activate(new ActivateLanguage
            {
                SiteId = siteId,
                Id = englishLanguageId
            }, _activateLanguageValidator);

            _languageRepository.Create(language);

            // ===== Pages ===== //

            var pagePermisisons = new List<PagePermission>
            {
                new PagePermission
                {
                    RoleId = Everyone.Id,
                    Type = PermissionType.View
                }
            };

            pagePermisisons.Add(new PagePermission
            {
                RoleId = Administrator.Id,
                Type = PermissionType.View
            });

            pagePermisisons.Add(new PagePermission
            {
                RoleId = Administrator.Id,
                Type = PermissionType.Edit
            });

            var homePage = Page.CreateNew(new CreatePage
            {
                SiteId = siteId,
                Id = homePageId,
                Name = "Home",
                Url = "home",
                PageLocalisations = new List<PageLocalisation>
                {
                    new PageLocalisation
                    {
                        LanguageId = englishLanguageId
                    }
                },
                PagePermissions = pagePermisisons
            }, _createPageValidator);

            homePage.Activate(new ActivatePage
            {
                SiteId = siteId,
                Id = homePageId
            }, _activatePageValidator);

            _pageRepository.Create(homePage);

            // ===== Modules ===== //

            var textModuleType = _moduleTypeRepository.GetByName("Text");

            // Content Zone Module

            var contentModuleId = Guid.NewGuid();

            var contentModule = Module.CreateNew(new CreateModule
            {
                SiteId = siteId,
                ModuleTypeId = textModuleType.Id,
                Id = contentModuleId,
                Title = "Content Module"
            }, _createModuleValidator);

            _moduleRepository.Create(contentModule);

            // Left Zone Module

            var leftModuleId = Guid.NewGuid();

            var leftModule = Module.CreateNew(new CreateModule
            {
                SiteId = siteId,
                ModuleTypeId = textModuleType.Id,
                Id = leftModuleId,
                Title = "Left Module"
            }, _createModuleValidator);

            _moduleRepository.Create(leftModule);

            // Right Zone Module

            var rightModuleId = Guid.NewGuid();

            var rightModule = Module.CreateNew(new CreateModule
            {
                SiteId = siteId,
                ModuleTypeId = textModuleType.Id,
                Id = rightModuleId,
                Title = "Right Module"
            }, _createModuleValidator);

            _moduleRepository.Create(rightModule);

            // Update Home Page

            var pageModulePermisisons = new List<PageModulePermission>
            {
                new PageModulePermission
                {
                    RoleId = Everyone.Id,
                    Type = PermissionType.View
                }
            };

            pageModulePermisisons.Add(new PageModulePermission
            {
                RoleId = Administrator.Id,
                Type = PermissionType.View
            });

            pageModulePermisisons.Add(new PageModulePermission
            {
                RoleId = Administrator.Id,
                Type = PermissionType.Edit
            });

            homePage.AddModule(new AddPageModule
            {
                SiteId = siteId,
                PageId = homePageId,
                ModuleId = contentModuleId,
                PageModuleId = Guid.NewGuid(),
                Title = "Content",
                Zone = "Content",
                SortOrder = 1,
                PageModulePermissions = pageModulePermisisons
            }, _addPageModuleValidator);

            homePage.AddModule(new AddPageModule
            {
                SiteId = siteId,
                PageId = homePageId,
                ModuleId = leftModuleId,
                PageModuleId = Guid.NewGuid(),
                Title = "Left",
                Zone = "Left",
                SortOrder = 1,
                PageModulePermissions = pageModulePermisisons
            }, _addPageModuleValidator);

            homePage.AddModule(new AddPageModule
            {
                SiteId = siteId,
                PageId = homePageId,
                ModuleId = rightModuleId,
                PageModuleId = Guid.NewGuid(),
                Title = "Right",
                Zone = "Right",
                SortOrder = 1,
                PageModulePermissions = pageModulePermisisons
            }, _addPageModuleValidator);

            _pageRepository.Update(homePage);

            // ===== Menus ===== //

            var mainMenu = Menu.CreateNew(new CreateMenu
            {
                SiteId = siteId,
                Id = mainMenuId,
                Name = "Main"
            }, _createMenuValidator);

            mainMenu.AddMenuItem(new AddMenuItem
            {
                SiteId = siteId,
                MenuId = mainMenuId,
                MenuItemId = Guid.NewGuid(),
                Type = MenuItemType.Page,
                PageId = homePageId,
                Link = string.Empty,
                Text = "Home",
                Title = "Home Page",
                MenuItemLocalisations = new List<MenuItemLocalisation>
                {
                    new MenuItemLocalisation
                    {
                        LanguageId = englishLanguageId
                    }
                },
                MenuItemPermissions = new List<MenuItemPermission>
                {
                    new MenuItemPermission
                    {
                        RoleId = Everyone.Id
                    }
                }
            }, _addMenuItemValidator);

            _menuRepository.Create(mainMenu);

            // ===== Update Site ===== //

            site.UpdateDetails(new UpdateSiteDetails
            {
                SiteId = siteId,
                HomePageId = homePageId,
                Title = "Weapsy",
                MetaDescription = "Weapsy",
                MetaKeywords = "Weapsy",
                SiteLocalisations = new List<SiteLocalisation>
                {
                    new SiteLocalisation
                    {
                        SiteId = siteId,
                        LanguageId = englishLanguageId
                    }
                }
            }, _updateSiteDetailsValidator);

            _siteRepository.Update(site);
        }

        public async Task EnsureSiteInstalled(string name)
        {
            if (!await _dispatcher.GetResultAsync<IsSiteInstalled, bool>(new IsSiteInstalled { Name = name }))
                await InstallSite(name);
        }

        private async Task InstallSite(string name)
        {
            var siteId = Guid.NewGuid();
            var englishLanguageId = Guid.NewGuid();
            var mainMenuId = Guid.NewGuid();
            var homePageId = Guid.NewGuid();

            _dispatcher.SendAndPublish<CreateSite, Site>(new CreateSite
            {
                Id = siteId,
                Name = "Default"
            });

            //.....
        }
    }
}
