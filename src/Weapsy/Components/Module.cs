using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Weapsy.Domain.Pages;
using Weapsy.Mvc.Components;
using Weapsy.Mvc.Context;
using Weapsy.Reporting.Pages;
using Weapsy.Services.Identity;

namespace Weapsy.Components
{
    [ViewComponent(Name = "Module")]
    public class ModuleViewComponent : BaseViewComponent
    {
        private readonly IUserService _userService;

        public ModuleViewComponent(IUserService userService,
            IContextService contextService)
            : base(contextService)
        {
            _userService = userService;
        }

        public async Task<IViewComponentResult> InvokeAsync(ModuleModel model)
        {
            if (!_userService.IsUserAuthorized(User, model.Roles[PermissionType.View]))
                return Content(string.Empty);

            var viewName = !string.IsNullOrEmpty(model.Template.ViewName)
                ? model.Template.ViewName
                : "Default";

            return View(viewName, model);
        }
    }
}
