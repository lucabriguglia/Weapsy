using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Weapsy.Mvc.Components;
using Weapsy.Mvc.Context;
using Weapsy.Reporting.Pages;

namespace Weapsy.Components
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
            // to do: check if a view with the same name of the zone exists and render it instead of Default
            return View("Default", model);
        }
    }
}
