using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
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
        private readonly UserManager<IdentityUser> _userManager;

        public UserController(UserManager<IdentityUser> userManager,
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

        public async Task<IActionResult> Save(IdentityUser model)
        {
            throw new NotImplementedException();
        }

        public async Task<IActionResult> Edit(string id)
        {
            var model = await _userManager.FindByIdAsync(id);

            if (model == null)
                return NotFound();

            return View(model);
        }

        public async Task<IActionResult> Update(IdentityUser model)
        {
            if (!ModelState.IsValid)
            {
                return View(nameof(Edit), model);
            }
            var result = await _userManager.UpdateAsync(model);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Index));
            }
            AddErrors(result);
            return View(nameof(Edit), model);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
    }
}
