using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Weapsy.Domain.Languages;
using Weapsy.Domain.Menus;
using Weapsy.Infrastructure.Identity;
using Weapsy.Infrastructure.Queries;
using Weapsy.Reporting.Menus;
using Weapsy.Reporting.Menus.Queries;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Weapsy.Data.Identity;
using MenuItem = Weapsy.Data.Entities.MenuItem;

namespace Weapsy.Data.Reporting.Menus
{
    public class GetItemsForAdminHandler : IQueryHandlerAsync<GetItemsForAdmin, IEnumerable<MenuItemAdminListModel>>
    {
        private readonly IDbContextFactory _contextFactory;
        private readonly IMapper _mapper;
        private readonly IRoleService _roleService;

        public GetItemsForAdminHandler(IDbContextFactory contextFactory, IMapper mapper, IRoleService roleService)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
            _roleService = roleService;
        }

        public async Task<IEnumerable<MenuItemAdminListModel>> RetrieveAsync(GetItemsForAdmin query)
        {
            using (var context = _contextFactory.Create())
            {
                var menu =
                    context.Menus.Include(x => x.MenuItems).FirstOrDefault(
                        x => x.SiteId == query.SiteId && x.Id == query.Id && x.Status != MenuStatus.Deleted);

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
    }
}
