using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Weapsy.Domain.Pages;
using Weapsy.Mvc.Components;
using Weapsy.Mvc.Context;
using Weapsy.Reporting.Pages;
using Weapsy.Services.Security;

namespace Weapsy.Web.Components
{
    [ViewComponent(Name = "ModuleActions")]
    public class ModuleActionsViewComponent : BaseViewComponent
    {
        private readonly IContextService _contextService;
        private readonly ISecurityService _securityService;

        public ModuleActionsViewComponent(IContextService contextService,
            ISecurityService securityService)
            : base(contextService)
        {
            _contextService = contextService;
            _securityService = securityService;
        }

        public async Task<IViewComponentResult> InvokeAsync(ModuleModel model)
        {
            if (!_securityService.IsUserAuthorized(User, model.Roles[PermissionType.Edit]))
                return Content(string.Empty);

            return View(model);
        }
    }
}
