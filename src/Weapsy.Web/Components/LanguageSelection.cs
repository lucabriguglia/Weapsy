using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Weapsy.Framework.Queries;
using Weapsy.Mvc.Components;
using Weapsy.Mvc.Context;
using Weapsy.Reporting.Languages;
using Weapsy.Reporting.Languages.Queries;

namespace Weapsy.Web.Components
{
    [ViewComponent(Name = "LanguageSelection")]
    public class LanguageSelectionViewComponent : BaseViewComponent
    {
        private readonly IQueryDispatcher _queryDispatcher;

        public LanguageSelectionViewComponent(IQueryDispatcher queryDispatcher,
            IContextService contextService)
            : base(contextService)
        {
            _queryDispatcher = queryDispatcher;
        }

        public async Task<IViewComponentResult> InvokeAsync(string viewName = "Default")
        {
            var languages = await _queryDispatcher.DispatchAsync<GetAllActive, IEnumerable<LanguageInfo>>(new GetAllActive { SiteId = SiteId });
            return View(viewName, languages);
        }
    }
}
