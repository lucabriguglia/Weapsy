using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Weapsy.Infrastructure.Caching;
using Weapsy.Domain.Languages;
using Weapsy.Domain.Menus;
using Weapsy.Domain.Pages;
using Weapsy.Infrastructure.Identity;
using Weapsy.Reporting.Menus;
using Weapsy.Services.Identity;

namespace Weapsy.Reporting.Data.Default.Menus
{
    public class MenuFacade : IMenuFacade
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IPageRepository _pageRepository;
        private readonly ILanguageRepository _languageRepository;
        private readonly ICacheManager _cacheManager;
        private readonly IMapper _mapper;
        private readonly IRoleService _roleService;

        public MenuFacade(IMenuRepository menuRepository, 
            IPageRepository pageRepository, 
            ILanguageRepository languageRepository, 
            ICacheManager cacheManager, 
            IMapper mapper, 
            IRoleService roleService)
        {
            _menuRepository = menuRepository;
            _pageRepository = pageRepository;
            _languageRepository = languageRepository;
            _cacheManager = cacheManager;
            _mapper = mapper;
            _roleService = roleService;
        }

        public MenuViewModel GetByName(Guid siteId, string name, Guid languageId = new Guid())
        {
            return _cacheManager.Get(string.Format(CacheKeys.MenuCacheKey, siteId, name, languageId), () =>
            {
                var menu = _menuRepository.GetByName(siteId, name);

                if (menu == null)
                    return new MenuViewModel();

                Language language = null;

                if (languageId != Guid.Empty)
                    language = _languageRepository.GetById(languageId);

                var menuModel = new MenuViewModel
                {
                    Name = menu.Name,
                    MenuItems = PopulateMenuItems(menu.MenuItems, Guid.Empty, language)
                };

                return menuModel;
            });
        }

        // UNDER DEVELOPMENT/REFACTORING
        private List<MenuViewModel.MenuItem> PopulateMenuItems(IEnumerable<MenuItem> source, Guid parentId, Language language)
        {
            var result = new List<MenuViewModel.MenuItem>();

            var menuItems = source as IList<MenuItem> ?? source.ToList();

            foreach (var menuItem in menuItems.Where(x => x.ParentId == parentId).OrderBy(x => x.SortOrder).ToList())
            {
                var menuItemRoleIds = menuItem.MenuItemPermissions.Select(x => x.RoleId);

                var text = menuItem.Text;
                var title = menuItem.Title;
                var url = "#";

                if (language != null)
                {
                    var menuItemLocalisation = menuItem.MenuItemLocalisations.FirstOrDefault(x => x.LanguageId == language.Id);

                    if (menuItemLocalisation != null)
                    {
                        text = !string.IsNullOrWhiteSpace(menuItemLocalisation.Text) ? menuItemLocalisation.Text : text;
                        title = !string.IsNullOrWhiteSpace(menuItemLocalisation.Title) ? menuItemLocalisation.Title : title;
                    }
                }

                if (menuItem.Type == MenuItemType.Page)
                {
                    var page = _pageRepository.GetById(menuItem.PageId);

                    if (page == null)
                        continue;

                    if (language == null)
                    {
                        url = $"/{page.Url}";
                    }
                    else
                    {
                        var pageLocalisation = page.PageLocalisations.FirstOrDefault(x => x.LanguageId == language.Id);
                        if (pageLocalisation != null)
                            url = !string.IsNullOrEmpty(pageLocalisation.Url)
                                ? $"/{pageLocalisation.Url}"
                                : $"/{page.Url}";

                        //if (site.AddLanguageToUrl)
                        //    url = $"/{language.Url}" + url;
                    }

                    menuItemRoleIds = page.PagePermissions.Where(x => x.Type == PermissionType.View).Select(x => x.RoleId);
                }
                else if (menuItem.Type == MenuItemType.Link && !string.IsNullOrWhiteSpace(menuItem.Link))
                {
                    url = menuItem.Link;
                }

                var menuItemRoles = _roleService.GetRolesFromIds(menuItemRoleIds);

                var menuItemModel = new MenuViewModel.MenuItem
                {
                    Text = text,
                    Title = title,
                    Url = url,
                    ViewRoles = menuItemRoles.Select(x => x.Name)
                };

                menuItemModel.Children.AddRange(PopulateMenuItems(menuItems, menuItem.Id, language));

                result.Add(menuItemModel);
            }

            return result;
        }

        public MenuItemAdminModel GetItemForAdmin(Guid siteId, Guid menuId, Guid menuItemId)
        {
            var menu = _menuRepository.GetById(siteId, menuId);

            if (menu == null)
                return new MenuItemAdminModel();

            var menuItem = menu.MenuItems.FirstOrDefault(x => x.Id == menuItemId);

            if (menuItem == null)
                return new MenuItemAdminModel();

            var result = new MenuItemAdminModel
            {
                Id = menuItem.Id,
                Type = menuItem.Type,
                PageId = menuItem.PageId,
                Link = menuItem.Link,
                Text = menuItem.Text,
                Title = menuItem.Title
            };

            var languages = _languageRepository.GetAll(siteId);

            foreach (var language in languages)
            {
                var text = string.Empty;
                var title = string.Empty;

                var existingLocalisation = menuItem.MenuItemLocalisations.FirstOrDefault(x => x.LanguageId == language.Id);

                if (existingLocalisation != null)
                {
                    text = existingLocalisation.Text;
                    title = existingLocalisation.Title;
                }

                result.MenuItemLocalisations.Add(new MenuItemAdminModel.MenuItemLocalisation
                {
                    MenuItemId = menuItem.Id,
                    LanguageId = language.Id,
                    LanguageName = language.Name,
                    LanguageStatus = language.Status,
                    Text = text,
                    Title = title
                });
            }

            foreach (var role in _roleService.GetAllRoles())
            {
                bool selected = menuItem.MenuItemPermissions.FirstOrDefault(x => x.RoleId == role.Id) != null;

                result.MenuItemPermissions.Add(new MenuItemAdminModel.MenuItemPermission
                {
                    MenuItemId = menuItem.Id,
                    RoleId = role.Id,
                    RoleName = role.Name,
                    Selected = selected || role.Name == DefaultRoleNames.Administrator,
                    Disabled = role.Name == DefaultRoleNames.Administrator
                });
            }

            return result;
        }

        public IEnumerable<MenuItemAdminListModel> GetMenuItemsForAdminList(Guid siteId, Guid menuId)
        {
            var menu = _menuRepository.GetById(siteId, menuId);

            if (menu == null)
                return new List<MenuItemAdminListModel>();

            var menuItems = menu.MenuItems.ToList();

            return PopulateMenuItemsForAdmin(menuItems, Guid.Empty);
        }

        private List<MenuItemAdminListModel> PopulateMenuItemsForAdmin(List<MenuItem> source, Guid parentId)
        {
            var result = new List<MenuItemAdminListModel>();

            foreach (var menuItem in source.Where(x => x.ParentId == parentId).OrderBy(x => x.SortOrder).ToList())
            {
                var menuItemModel = new MenuItemAdminListModel
                {
                    Id = menuItem.Id,
                    ParentId = menuItem.ParentId,
                    SortOrder = menuItem.SortOrder,
                    Text = menuItem.Text,
                    Type = menuItem.Type
                };

                menuItemModel.MenuItems.AddRange(PopulateMenuItemsForAdmin(source, menuItem.Id));

                result.Add(menuItemModel);
            }

            return result;
        }

        public IEnumerable<MenuAdminModel> GetAllForAdmin(Guid siteId)
        {
            var menus = _menuRepository.GetAll(siteId);
            return _mapper.Map<IEnumerable<MenuAdminModel>>(menus);
        }

        public MenuAdminModel GetForAdmin(Guid siteId, Guid id)
        {
            throw new NotImplementedException();
        }
    }
}

