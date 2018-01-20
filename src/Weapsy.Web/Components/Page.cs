using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Weapsy.Mvc.Components;
using Weapsy.Mvc.Context;
using Weapsy.Reporting.Pages;

namespace Weapsy.Web.Components
{
    [ViewComponent(Name = "Page")]
    public class PageViewComponent : BaseViewComponent
    {
        private readonly IContextService _contextService;

        public PageViewComponent(IContextService contextService)
            : base(contextService)
        {
            _contextService = contextService;
        }

        public async Task<IViewComponentResult> InvokeAsync(PageInfo model)
        {
            var viewName = !string.IsNullOrEmpty(model.Page.Template)
                ? model.Page.Template
                : "Default";

            return View(viewName, model);
        }
    }
}
