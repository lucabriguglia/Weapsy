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
        private readonly IMenuFacade _menuFacade;

        public NavigationViewComponent(IContextService contextService, IMenuFacade menuFacade)
            : base(contextService)
        {
            _menuFacade = menuFacade;
        }

        public async Task<IViewComponentResult> InvokeAsync(string name, string view = "Default")
        {
            var viewModel = await _menuFacade.GetByNameAsync(SiteId, name/*, SiteInfo.LanguageId*/);
            return View(viewModel);
        }
    }
}
