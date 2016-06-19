using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Weapsy.Mvc.Components;
using Weapsy.Mvc.Context;
using Weapsy.Reporting.ModuleTypes;

namespace Weapsy.Components
{
    [ViewComponent(Name = "ControlPanel")]
    public class ControlPanelViewComponent : BaseViewComponent
    {
        private readonly IContextService _contextService;
        private readonly IModuleTypeFacade _moduleTypeFacade;

        public ControlPanelViewComponent(IContextService contextService,
            IModuleTypeFacade moduleTypeFacade)
            : base(contextService)
        {
            _contextService = contextService;
            _moduleTypeFacade = moduleTypeFacade;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = _moduleTypeFacade.GetControlPanelModel();
            return View(model);
        }
    }
}
