using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Weapsy.Framework.Queries;
using Weapsy.Mvc.Components;
using Weapsy.Mvc.Context;
using Weapsy.Reporting.Menus;
using Weapsy.Reporting.Menus.Queries;
using Weapsy.Services.Security;

namespace Weapsy.Components
{
    [ViewComponent(Name = "Navigation")]
    public class NavigationViewComponent : BaseViewComponent
    {
        private readonly IContextService _contextService;
        private readonly IQueryDispatcher _queryDispatcher;
        private readonly ISecurityService _securityService;

        public NavigationViewComponent(IContextService contextService,
            IQueryDispatcher queryDispatcher,
            ISecurityService securityService)
            : base(contextService)
        {
            _contextService = contextService;
            _queryDispatcher = queryDispatcher;
            _securityService = securityService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string name, string viewName = "Default")
        {
            var viewModel = await _queryDispatcher.DispatchAsync<GetViewModel, MenuViewModel>(new GetViewModel
            {
                SiteId = SiteId,
                Name = name,
                LanguageId = _contextService.GetCurrentLanguageInfo().Id
            });

            var menuItemsToRemove = new List<MenuViewModel.MenuItem>();

            foreach (var menuItemViewModel in viewModel.MenuItems)
                if (!_securityService.IsUserAuthorized(User, menuItemViewModel.ViewRoles))
                    menuItemsToRemove.Add(menuItemViewModel);

            if (menuItemsToRemove.Any())
                foreach (var menuItemToRemove in menuItemsToRemove)
                    viewModel.MenuItems.Remove(menuItemToRemove);

            return View(viewName, viewModel);
        }
    }
}
