using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Weapsy.Data.TempIdentity;
using Weapsy.Mvc.Context;
using Weapsy.Mvc.Controllers;

namespace Weapsy.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RoleController : BaseAdminController
    {
        private readonly RoleManager<ApplicationRole> _roleManager;

        public RoleController(RoleManager<ApplicationRole> roleManager,
            IContextService contextService)
            : base(contextService)
        {
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            if (_roleManager.SupportsQueryableRoles)
            {
                var model = _roleManager.Roles.OrderBy(x => x.Name).ToList();
                return View(model);
            }
            return View(new List<ApplicationRole>());
        }

        public IActionResult Create()
        {
            return View(new ApplicationRole());
        }

        public async Task<IActionResult> Save(ApplicationRole role)
        {
            var result = await _roleManager.CreateAsync(role);

            if (!result.Succeeded)
                throw new Exception(GetErrorMessage(result));

            return new NoContentResult();
        }

        public async Task<IActionResult> Edit(string id)
        {
            var model = await _roleManager.FindByIdAsync(id);

            if (model == null)
                return NotFound();

            return View(model);
        }

        public async Task<IActionResult> Update(ApplicationRole model)
        {
            var role = await _roleManager.FindByIdAsync(model.Id.ToString());

            if (role == null)
                throw new Exception("Role not found.");

            role.Name = model.Name;

            var result = await _roleManager.UpdateAsync(role);

            if (!result.Succeeded)
                throw new Exception(GetErrorMessage(result));

            return new NoContentResult();
        }

        private string GetErrorMessage(IdentityResult result)
        {
            var builder = new StringBuilder();

            foreach (var error in result.Errors)
                builder.AppendLine(error.Description);

            return builder.ToString();
        }
    }
}
