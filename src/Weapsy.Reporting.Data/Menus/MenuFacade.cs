using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Weapsy.Data;
using Weapsy.Domain.Languages;
using Weapsy.Domain.Menus;
using Weapsy.Domain.Pages;
using Weapsy.Infrastructure.Caching;
using Weapsy.Infrastructure.Identity;
using Weapsy.Reporting.Menus;
using Weapsy.Services.Identity;
using Language = Weapsy.Data.Entities.Language;
using Menu = Weapsy.Data.Entities.Menu;
using MenuItem = Weapsy.Data.Entities.MenuItem;

namespace Weapsy.Reporting.Data.Menus
{
    public class MenuFacade : IMenuFacade
    {
        private readonly IDbContextFactory _dbContextFactory;
        private readonly ICacheManager _cacheManager;
        private readonly IMapper _mapper;
        private readonly IRoleService _roleService;

        public MenuFacade(IDbContextFactory dbContextFactory, 
            ICacheManager cacheManager, 
            IMapper mapper, 
            IRoleService roleService)
        {
            _dbContextFactory = dbContextFactory;
            _cacheManager = cacheManager;
            _mapper = mapper;
            _roleService = roleService;
        }

        public MenuViewModel GetByName(Guid siteId, string name, Guid languageId = new Guid())
        {
            return _cacheManager.Get(string.Format(CacheKeys.MenuCacheKey, siteId, name, languageId), () =>
            {
                using (var context = _dbContextFactory.Create())
                {
                    var menu = context.Menus.FirstOrDefault(x => x.SiteId == siteId && x.Name == name && x.Status != MenuStatus.Deleted);

                    if (menu == null)
                        return new MenuViewModel();

                    LoadMenuItems(context, menu);

                    var language = languageId != Guid.Empty 
                        ? context.Languages.FirstOrDefault(x => x.SiteId == siteId && x.Id == languageId && x.Status == LanguageStatus.Active)
                        : null;

                    bool addLanguageSlug = context.Sites.Where(x => x.Id == siteId).Select(site => site.AddLanguageSlug).FirstOrDefault();

                    var menuModel = new MenuViewModel
                    {
                        Name = menu.Name,
                        MenuItems = PopulateMenuItems(context, menu.MenuItems, Guid.Empty, language, addLanguageSlug)
                    };

                    return menuModel;
                }
            });
        }

        // UNDER DEVELOPMENT/REFACTORING
        private List<MenuViewModel.MenuItem> PopulateMenuItems(WeapsyDbContext context, IEnumerable<MenuItem> source, Guid parentId, Language language, bool addLanguageSlug)
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
                    var page = context.Pages
                        .Include(x => x.PageLocalisations)
                        .Include(x => x.PagePermissions)
                        .FirstOrDefault(x => x.Id == menuItem.PageId && x.Status != PageStatus.Deleted);

                    if (page == null)
                        continue;

                    if (language == null)
                    {
                        url = $"/{page.Url}";
                    }
                    else
                    {
                        var pageLocalisation = page.PageLocalisations.FirstOrDefault(x => x.LanguageId == language.Id);

                        url = pageLocalisation == null
                            ? $"/{page.Url}"
                            : !string.IsNullOrEmpty(pageLocalisation.Url)
                                ? $"/{pageLocalisation.Url}"
                                : $"/{page.Url}";

                        if (addLanguageSlug)
                            url = $"/{language.Url}" + url;
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

                menuItemModel.Children.AddRange(PopulateMenuItems(context, menuItems, menuItem.Id, language, addLanguageSlug));

                result.Add(menuItemModel);
            }

            return result;
        }

        public MenuItemAdminModel GetItemForAdmin(Guid siteId, Guid menuId, Guid menuItemId)
        {
            using (var context = _dbContextFactory.Create())
            {
                var menu = context.Menus.FirstOrDefault(x => x.SiteId == siteId && x.Id == menuId && x.Status != MenuStatus.Deleted);

                if (menu == null)
                    return new MenuItemAdminModel();

                var menuItem = context.MenuItems
                    .Include(x => x.MenuItemLocalisations)
                    .Include(x => x.MenuItemPermissions)
                    .FirstOrDefault(x => x.MenuId == menuId && x.Id == menuItemId && x.Status != MenuItemStatus.Deleted);

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

                var languages = context.Languages
                    .Where(x => x.SiteId == siteId && x.Status != LanguageStatus.Deleted)
                    .OrderBy(x => x.SortOrder)
                    .ToList();

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
        }

        public MenuItemAdminModel GetDefaultItemForAdmin(Guid siteId, Guid menuId)
        {
            using (var context = _dbContextFactory.Create())
            {
                var menu = context.Menus.FirstOrDefault(x => x.SiteId == siteId && x.Id == menuId && x.Status != MenuStatus.Deleted);

                if (menu == null)
                    return new MenuItemAdminModel();

                var result = new MenuItemAdminModel();

                var languages = context.Languages
                    .Where(x => x.SiteId == siteId && x.Status != LanguageStatus.Deleted)
                    .OrderBy(x => x.SortOrder)
                    .ToList();

                foreach (var language in languages)
                {
                    result.MenuItemLocalisations.Add(new MenuItemAdminModel.MenuItemLocalisation
                    {
                        LanguageId = language.Id,
                        LanguageName = language.Name,
                        LanguageStatus = language.Status,
                        Text = string.Empty,
                        Title = string.Empty
                    });
                }

                foreach (var role in _roleService.GetAllRoles())
                {
                    result.MenuItemPermissions.Add(new MenuItemAdminModel.MenuItemPermission
                    {
                        RoleId = role.Id,
                        RoleName = role.Name,
                        Selected = role.Name == DefaultRoleNames.Administrator,
                        Disabled = role.Name == DefaultRoleNames.Administrator
                    });
                }

                return result;
            }
        }

        public IEnumerable<MenuItemAdminListModel> GetMenuItemsForAdminList(Guid siteId, Guid menuId)
        {
            using (var context = _dbContextFactory.Create())
            {
                var menu =
                    context.Menus.Include(x => x.MenuItems).FirstOrDefault(
                        x => x.SiteId == siteId && x.Id == menuId && x.Status != MenuStatus.Deleted);

                if (menu == null)
                    return new List<MenuItemAdminListModel>();

                var menuItems = menu.MenuItems.ToList();

                return PopulateMenuItemsForAdmin(menuItems, Guid.Empty);
            }
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
            using (var context = _dbContextFactory.Create())
            {
                var menus =
                    context.Menus.Include(x => x.MenuItems).Where(
                        x => x.SiteId == siteId && x.Status != MenuStatus.Deleted)
                        .ToList();

                return _mapper.Map<IEnumerable<MenuAdminModel>>(menus);
            }            
        }

        public MenuAdminModel GetForAdmin(Guid siteId, Guid id)
        {
            throw new NotImplementedException();
        }

        private void LoadMenuItems(WeapsyDbContext context, Menu menu)
        {
            if (menu == null)
                return;

            menu.MenuItems = context.MenuItems
                .Include(x => x.MenuItemLocalisations)
                .Include(x => x.MenuItemPermissions)
                .Where(x => x.MenuId == menu.Id && x.Status != MenuItemStatus.Deleted)
                .ToList();
        }
    }
}
