using System;
using System.Collections.Generic;
using Weapsy.Core.Dispatcher;
using Weapsy.Domain.Model.Languages;
using Weapsy.Domain.Model.Languages.Commands;
using Weapsy.Domain.Model.Menus;
using Weapsy.Domain.Model.Menus.Commands;
using Weapsy.Domain.Model.Pages;
using Weapsy.Domain.Model.Pages.Commands;
using Weapsy.Domain.Model.Sites;
using Weapsy.Domain.Model.Sites.Commands;

namespace Weapsy.Domain.Services.Installation
{
    public class SiteInstallationService2 : ISiteInstallationService
    {
        private readonly ICommandSender _commandSender;

        public SiteInstallationService2(ICommandSender commandSender)
        {
            _commandSender = commandSender;
        }

        public void InstallDefaultSite()
        {
            var siteId = Guid.NewGuid();
            var englishLanguageId = Guid.NewGuid();
            var mainMenuId = Guid.NewGuid();
            var homePageId = Guid.NewGuid();
            
            _commandSender.Send<CreateSite, Site>(new CreateSite
            {
                Id = siteId,
                Name = "Default"
            }, false);

            _commandSender.Send<CreateLanguage, Language>(new CreateLanguage
            {
                SiteId = siteId,
                Id = englishLanguageId,
                Name = "English",
                CultureName = "en",
                Url = "en"
            }, false);

            _commandSender.Send<CreatePage, Page>(new CreatePage
            {
                SiteId = siteId,
                Id = homePageId,
                Name = "Home",
                Url = "home",
                PageLocalisations = new List<PageDetails.PageLocalisation>
                {
                    new PageDetails.PageLocalisation
                    {
                        LanguageId = englishLanguageId
                    }
                }
            }, false);

            _commandSender.Send<ActivatePage, Page>(new ActivatePage
            {
                SiteId = siteId,
                Id = homePageId
            }, false);

            _commandSender.Send<CreateMenu, Menu>(new CreateMenu
            {
                SiteId = siteId,
                Id = mainMenuId,
                Name = "Main"
            }, false);

            _commandSender.Send<AddMenuItem, Menu>(new AddMenuItem
            {
                SiteId = siteId,
                MenuId = mainMenuId,
                MenuItemId = Guid.NewGuid(),
                MenuItemType = MenuItemType.Page,
                PageId = homePageId,
                Link = string.Empty,
                Text = "Home",
                Title = "Home Page",
                MenuItemLocalisations = new List<MenuItemDetails.MenuItemLocalisation>
                {
                    new MenuItemDetails.MenuItemLocalisation
                    {
                        LanguageId = englishLanguageId
                    }
                }
            }, false);

            // to do: add module(s) to home page
        }
    }
}
