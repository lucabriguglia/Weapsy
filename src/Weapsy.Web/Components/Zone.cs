using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Weapsy.Mvc.Components;
using Weapsy.Mvc.Context;
using Weapsy.Reporting.Pages;

namespace Weapsy.Web.Components
{
    [ViewComponent(Name = "Zone")]
    public class ZoneViewComponent : BaseViewComponent
    {
        private readonly IContextService _contextService;

        public ZoneViewComponent(IContextService contextService)
            : base(contextService)
        {
            _contextService = contextService;
        }

        public async Task<IViewComponentResult> InvokeAsync(ZoneModel model)
        {
            return View("Default", model);
        }
    }
}
