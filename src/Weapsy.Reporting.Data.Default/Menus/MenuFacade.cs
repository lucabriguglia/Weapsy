using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Weapsy.Infrastructure.Caching;
using Weapsy.Domain.Languages;
using Weapsy.Domain.Menus;
using Weapsy.Domain.Pages;
using Weapsy.Reporting.Menus;

namespace Weapsy.Reporting.Data.Default.Menus
{
    public class MenuFacade : IMenuFacade
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IPageRepository _pageRepository;
        private readonly ILanguageRepository _languageRepository;
        private readonly ICacheManager _cacheManager;
        private readonly IMapper _mapper;

        public MenuFacade(IMenuRepository menuRepository, 
            IPageRepository pageRepository, 
            ILanguageRepository languageRepository, 
            ICacheManager cacheManager, 
            IMapper mapper)
        {
            _menuRepository = menuRepository;
            _pageRepository = pageRepository;
            _languageRepository = languageRepository;
            _cacheManager = cacheManager;
            _mapper = mapper;
        }

        public async Task<MenuViewModel> GetByNameAsync(Guid siteId, string name, Guid languageId = new Guid())
        {
            return _cacheManager.Get(string.Format(CacheKeys.MenuCacheKey, siteId, name), () =>
            {
                var menu = _menuRepository.GetByName(siteId, name);

                if (menu == null)
                    return new MenuViewModel();

                var menuModel = new MenuViewModel { Name = menu.Name };

                menuModel.MenuItems = PopulateMenuItems(menu.MenuItems.Where(x => x.Status == MenuItemStatus.Active), Guid.Empty, languageId);

                return menuModel;
            });
        }

        private List<MenuViewModel.MenuItem> PopulateMenuItems(IEnumerable<MenuItem> source, Guid parentId, Guid languageId)
        {
            var result = new List<MenuViewModel.MenuItem>();

            foreach (var menuItem in source.Where(x => x.ParentId == parentId).OrderBy(x => x.SortOrder).ToList())
            {
                var text = menuItem.Text;
                var title = menuItem.Title;
                var url = "#";

                if (languageId != Guid.Empty)
                {
                    var menuItemLocalisation = menuItem.MenuItemLocalisations.FirstOrDefault(x => x.LanguageId == languageId);

                    if (menuItemLocalisation != null)
                    {
                        text = !string.IsNullOrWhiteSpace(menuItemLocalisation.Text) ? menuItemLocalisation.Text : text;
                        title = !string.IsNullOrWhiteSpace(menuItemLocalisation.Title) ? menuItemLocalisation.Title : title;
                    }
                }

                if (menuItem.MenuItemType == MenuItemType.Page)
                {
                    var page = _pageRepository.GetById(menuItem.PageId);

                    if (page == null || page.Status != PageStatus.Active)
                        continue;

                    url = $"/{page.Url}";
                }
                else if (menuItem.MenuItemType == MenuItemType.Link && !string.IsNullOrWhiteSpace(menuItem.Link))
                {
                    url = menuItem.Link;
                }

                var menuItemModel = new MenuViewModel.MenuItem
                {
                    Text = text,
                    Title = title,
                    Url = url
                };

                menuItemModel.Children.AddRange(PopulateMenuItems(source, menuItem.Id, languageId));

                result.Add(menuItemModel);
            }

            return result;
        }

        public async Task<MenuItemAdminModel> GetItemForAdminAsync(Guid siteId, Guid menuId, Guid menuItemId)
        {
            var menu = _menuRepository.GetById(siteId, menuId);

            if (menu == null || menu.Status == MenuStatus.Deleted)
                return new MenuItemAdminModel();

            var menuItem = menu.MenuItems.FirstOrDefault(x => x.Id == menuItemId);

            if (menuItem == null || menuItem.Status == MenuItemStatus.Deleted)
                return new MenuItemAdminModel();

            var result = new MenuItemAdminModel
            {
                Id = menuItem.Id,
                MenuItemType = menuItem.MenuItemType,
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
                    LanguageId = language.Id,
                    LanguageName = language.Name,
                    Text = text,
                    Title = title
                });
            }

            return result;
        }

        public async Task<IEnumerable<MenuItemAdminListModel>> GetMenuItemsForAdminListAsync(Guid siteId, Guid menuId)
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
                    MenuItemType = menuItem.MenuItemType
                };

                menuItemModel.MenuItems.AddRange(PopulateMenuItemsForAdmin(source, menuItem.Id));

                result.Add(menuItemModel);
            }

            return result;
        }

        public async Task<IEnumerable<MenuAdminModel>> GetAllForAdminAsync(Guid siteId)
        {
            var menus = _menuRepository.GetAll(siteId).Where(x => x.Status != MenuStatus.Deleted);
            return _mapper.Map<IEnumerable<MenuAdminModel>>(menus);
        }

        public Task<MenuAdminModel> GetForAdminAsync(Guid siteId, Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
