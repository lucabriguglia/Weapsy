using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Weapsy.Mvc.Components;
using Weapsy.Mvc.Context;
using Weapsy.Reporting.Pages;

namespace Weapsy.Components
{
    [ViewComponent(Name = "Module")]
    public class ModuleViewComponent : BaseViewComponent
    {
        private readonly IContextService _contextService;

        public ModuleViewComponent(IContextService contextService)
            : base(contextService)
        {
            _contextService = contextService;
        }

        public async Task<IViewComponentResult> InvokeAsync(ModuleModel model)
        {
            var viewName = !string.IsNullOrEmpty(model.Template.ViewName)
                ? model.Template.ViewName
                : "Default";

            return View(viewName, model);
        }
    }
}
