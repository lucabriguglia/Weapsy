using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Weapsy.Mvc.Components;
using Weapsy.Mvc.Context;
using Weapsy.Reporting.Languages;

namespace Weapsy.Components
{
    [ViewComponent(Name = "LanguageSelection")]
    public class LanguageSelectionViewComponent : BaseViewComponent
    {
        private readonly ILanguageFacade _languageFacade;

        public LanguageSelectionViewComponent(ILanguageFacade languageFacade,
            IContextService contextService)
            : base(contextService)
        {
            _languageFacade = languageFacade;
        }

        public async Task<IViewComponentResult> InvokeAsync(string viewName = "Default")
        {
            var languages = await Task.Run(() => _languageFacade.GetAllActiveAsync(SiteId));
            return View(viewName, languages);
        }
    }
}
