using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Weapsy.Mvc.Components;
using Weapsy.Mvc.Context;
using Weapsy.Reporting.Menus;
using Weapsy.Services.Identity;

namespace Weapsy.Components
{
    [ViewComponent(Name = "Navigation")]
    public class NavigationViewComponent : BaseViewComponent
    {
        private readonly IContextService _contextService;
        private readonly IMenuFacade _menuFacade;
        private readonly IUserService _userService;

        public NavigationViewComponent(IContextService contextService, 
            IMenuFacade menuFacade,
            IUserService userService)
            : base(contextService)
        {
            _contextService = contextService;
            _menuFacade = menuFacade;
            _userService = userService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string name, string viewName = "Default")
        {
            var viewModel = await Task.Run(() => _menuFacade.GetByName(SiteId, name, _contextService.GetCurrentLanguageInfo().Id));

            var menuItemsToRemove = new List<MenuViewModel.MenuItem>();

            foreach (var menuItemViewModel in viewModel.MenuItems)
                if (!_userService.IsUserAuthorized(User, menuItemViewModel.ViewRoles))
                    menuItemsToRemove.Add(menuItemViewModel);

            if (menuItemsToRemove.Any())
                foreach (var menuItemToRemove in menuItemsToRemove)
                    viewModel.MenuItems.Remove(menuItemToRemove);

            return View(viewName, viewModel);
        }
    }
}
