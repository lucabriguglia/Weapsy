using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Weapsy.Data.Identity;
using Weapsy.Domain.Languages;
using Weapsy.Domain.Menus;
using Weapsy.Domain.Pages;
using Weapsy.Infrastructure.Caching;
using Weapsy.Infrastructure.Identity;
using Weapsy.Reporting.Menus;
using Language = Weapsy.Data.Entities.Language;
using Menu = Weapsy.Data.Entities.Menu;
using MenuItem = Weapsy.Data.Entities.MenuItem;

namespace Weapsy.Data.Reporting.Menus
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
