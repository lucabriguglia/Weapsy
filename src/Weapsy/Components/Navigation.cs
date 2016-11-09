using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Weapsy.Mvc.Components;
using Weapsy.Mvc.Context;
using Weapsy.Reporting.Menus;

namespace Weapsy.Components
{
    [ViewComponent(Name = "Navigation")]
    public class NavigationViewComponent : BaseViewComponent
    {
        private readonly IContextService _contextService;
        private readonly IMenuFacade _menuFacade;
        
        public NavigationViewComponent(IContextService contextService, IMenuFacade menuFacade)
            : base(contextService)
        {
            _contextService = contextService;
            _menuFacade = menuFacade;
        }

        public async Task<IViewComponentResult> InvokeAsync(string name, string viewName = "Default")
        {
            var viewModel = await _menuFacade.GetByNameAsync(SiteId, name, _contextService.GetCurrentLanguageInfo().Id);
            return View(viewModel);
        }
    }
}
