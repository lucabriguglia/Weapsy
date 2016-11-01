using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Weapsy.Mvc.Components;
using Weapsy.Mvc.Context;
using Weapsy.Reporting.Pages;

namespace Weapsy.Components
{
    [ViewComponent(Name = "Page")]
    public class PageViewComponent : BaseViewComponent
    {
        private readonly IContextService _contextService;
        private readonly IPageFacade _pageFacade;

        public PageViewComponent(IContextService contextService, IPageFacade pageFacade)
            : base(contextService)
        {
            _contextService = contextService;
            _pageFacade = pageFacade;
        }

        public async Task<IViewComponentResult> InvokeAsync(PageInfo model)
        {
            var viewName = !string.IsNullOrEmpty(model.Template.ViewName)
                ? model.Template.ViewName
                : "Default";

            return View(viewName, model);
        }
    }
}
