using System;
using Weapsy.Mvc.Controllers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Weapsy.Reporting.Languages;
using Weapsy.Mvc.Context;

namespace Weapsy.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LanguageController : BaseAdminController
    {
        private readonly ILanguageFacade _languageFacade;

        public LanguageController(ILanguageFacade languageFacade,
            IContextService contextService)
            : base(contextService)
        {
            _languageFacade = languageFacade;
        }

        public async Task<IActionResult> Index()
        {
            var models = await _languageFacade.GetAllForAdminAsync(SiteId);
            return View(models);
        }

        public IActionResult Create()
        {
            var model = new LanguageAdminModel();
            return View(model);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var model = await _languageFacade.GetForAdminAsync(SiteId, id);

            if (model == null)
                return NotFound();

            return View(model);
        }
    }
}
