using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Weapsy.Cqrs;
using Weapsy.Mvc.Components;
using Weapsy.Mvc.Context;
using Weapsy.Reporting.Languages;
using Weapsy.Reporting.Languages.Queries;

namespace Weapsy.Web.Components
{
    [ViewComponent(Name = "LanguageSelection")]
    public class LanguageSelectionViewComponent : BaseViewComponent
    {
        private readonly IDispatcher _dispatcher;

        public LanguageSelectionViewComponent(IDispatcher dispatcher,
            IContextService contextService)
            : base(contextService)
        {
            _dispatcher = dispatcher;
        }

        public async Task<IViewComponentResult> InvokeAsync(string viewName = "Default")
        {
            var languages = await _dispatcher.GetResultAsync<GetAllActive, IEnumerable<LanguageInfo>>(new GetAllActive { SiteId = SiteId });
            return View(viewName, languages);
        }
    }
}
