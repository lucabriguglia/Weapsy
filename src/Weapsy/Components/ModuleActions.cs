using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Weapsy.Mvc.Components;
using Weapsy.Mvc.Context;
using Weapsy.Reporting.Pages;

namespace Weapsy.Components
{
    [ViewComponent(Name = "ModuleActions")]
    public class ModuleActionsViewComponent : BaseViewComponent
    {
        private readonly IContextService _contextService;

        public ModuleActionsViewComponent(IContextService contextService)
            : base(contextService)
        {
            _contextService = contextService;
        }

        public async Task<IViewComponentResult> InvokeAsync(ModuleModel model)
        {
            return View(model);
        }
    }
}
