using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Weapsy.Cqrs;
using Weapsy.Mvc.Components;
using Weapsy.Mvc.Context;
using Weapsy.Reporting.Apps;
using Weapsy.Reporting.Apps.Queries;

namespace Weapsy.Web.Components
{
    [ViewComponent(Name = "ControlPanel")]
    public class ControlPanelViewComponent : BaseViewComponent
    {
        private readonly IContextService _contextService;
        private readonly IDispatcher _dispatcher;

        public ControlPanelViewComponent(IContextService contextService,
            IDispatcher dispatcher)
            : base(contextService)
        {
            _contextService = contextService;
            _dispatcher = dispatcher;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = await _dispatcher.GetResultAsync<GetControlPanelModel, IEnumerable<ModuleTypeControlPanelModel>>(new GetControlPanelModel());
            return View(model);
        }
    }
}
