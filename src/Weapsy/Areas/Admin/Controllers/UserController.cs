using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Weapsy.Models;
using Weapsy.Mvc.Context;
using Weapsy.Mvc.Controllers;

namespace Weapsy.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : BaseAdminController
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(UserManager<ApplicationUser> userManager,
            IContextService contextService)
            : base(contextService)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var model = _userManager.Users.ToList();
            return View(model);
        }

        public async Task<IActionResult> Create()
        {
            return View(new ApplicationUser());
        }

        public async Task<IActionResult> Save(ApplicationUser model)
        {
            throw new NotImplementedException();
        }

        public async Task<IActionResult> Edit(string id)
        {
            var model = await _userManager.FindByIdAsync(id);
            if (model == null) return NotFound();
            return View(model);
        }

        public async Task<IActionResult> Update(ApplicationUser model)
        {
            throw new NotImplementedException();
        }
    }
}
