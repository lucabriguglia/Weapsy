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
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;

        public UserController(UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IUserService userService,
            IRoleService roleService,
            IContextService contextService)
            : base(contextService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _userService = userService;
            _roleService = roleService;
        }

        public IActionResult Index()
        {
            return View();
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
            var model = await _userService.GetUserRolesViewModelAsync(id);

            if (model == null)
                return NotFound();

            return View(model);
        }
    }
}
