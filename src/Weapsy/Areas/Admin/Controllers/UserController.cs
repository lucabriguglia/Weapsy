using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Weapsy.Mvc.Context;
using Weapsy.Mvc.Controllers;

namespace Weapsy.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : BaseAdminController
    {
        private readonly UserManager<IdentityUser> _userManager;

        public UserController(UserManager<IdentityUser> userManager,
            IContextService contextService)
            : base(contextService)
        {
            _userManager = userManager;
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
    }
}
