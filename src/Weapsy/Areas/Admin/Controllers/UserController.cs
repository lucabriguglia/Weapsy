using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Weapsy.Mvc.Context;
using Weapsy.Mvc.Controllers;
using Weapsy.Services.Identity;

namespace Weapsy.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : BaseAdminController
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IIdentityService _identityService;

        public UserController(UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IIdentityService identityService,
            IContextService contextService)
            : base(contextService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _identityService = identityService;
        }

        public IActionResult Index()
        {
            var model = _userManager.Users.ToList();
            return View(model);
        }

        public IActionResult Create()
        {
            return View(new IdentityUser());
        }

        public async Task<IActionResult> Edit(string id)
        {
            var model = await _userManager.FindByIdAsync(id);

            if (model == null)
                return NotFound();

            return View(model);
        }

        public async Task<IActionResult> Roles(string id)
        {
            var model = await _identityService.GetUserRolesViewModel(id);

            if (model == null)
                return NotFound();

            return View(model);
        }
    }
}
