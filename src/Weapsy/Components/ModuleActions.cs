using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Weapsy.Domain.Pages;
using Weapsy.Mvc.Components;
using Weapsy.Mvc.Context;
using Weapsy.Reporting.Pages;
using Weapsy.Services.Identity;

namespace Weapsy.Components
{
    [ViewComponent(Name = "ModuleActions")]
    public class ModuleActionsViewComponent : BaseViewComponent
    {
        private readonly IContextService _contextService;
        private readonly IUserService _userService;

        public ModuleActionsViewComponent(IContextService contextService, 
            IUserService userService)
            : base(contextService)
        {
            _contextService = contextService;
            _userService = userService;
        }

        public async Task<IViewComponentResult> InvokeAsync(ModuleModel model)
        {
            if (!_userService.IsUserAuthorized(User, model.Roles[PermissionType.Edit]))
                return Content(string.Empty);

            return View(model);
        }
    }
}
