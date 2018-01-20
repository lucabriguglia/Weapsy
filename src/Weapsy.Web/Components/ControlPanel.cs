using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Weapsy.Framework.Queries;
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
        private readonly IQueryDispatcher _queryDispatcher;

        public ControlPanelViewComponent(IContextService contextService,
            IQueryDispatcher queryDispatcher)
            : base(contextService)
        {
            _contextService = contextService;
            _queryDispatcher = queryDispatcher;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = await _queryDispatcher.DispatchAsync<GetControlPanelModel, IEnumerable<ModuleTypeControlPanelModel>>(new GetControlPanelModel());
            return View(model);
        }
    }
}
