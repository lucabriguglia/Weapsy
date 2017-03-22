using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Weapsy.Domain.Pages;
using Weapsy.Mvc.Components;
using Weapsy.Mvc.Context;
using Weapsy.Reporting.Pages;
using Weapsy.Services.Security;

namespace Weapsy.Components
{
    [ViewComponent(Name = "Module")]
    public class ModuleViewComponent : BaseViewComponent
    {
        private readonly ISecurityService _securityService;

        public ModuleViewComponent(ISecurityService securityService,
            IContextService contextService)
            : base(contextService)
        {
            _securityService = securityService;
        }

        public async Task<IViewComponentResult> InvokeAsync(ModuleModel model)
        {
            if (!_securityService.IsUserAuthorized(User, model.Roles[PermissionType.View]))
                return Content(string.Empty);

            var viewName = !string.IsNullOrEmpty(model.Template)
                ? model.Template
                : "Default";

            return View(viewName, model);
        }
    }
}
