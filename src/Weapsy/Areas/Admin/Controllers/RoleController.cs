using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Weapsy.Data.Entities;
using Weapsy.Data.Identity;
using Weapsy.Mvc.Context;
using Weapsy.Mvc.Controllers;

namespace Weapsy.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RoleController : BaseAdminController
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly IRoleService _roleService;

        public RoleController(RoleManager<Role> roleManager,
            IRoleService roleService,
            IContextService contextService)
            : base(contextService)
        {
            _roleManager = roleManager;
            _roleService = roleService;
        }

        public IActionResult Index()
        {
            if (_roleManager.SupportsQueryableRoles)
            {
                var model = _roleManager.Roles.OrderBy(x => x.Name).ToList();
                return View(model);
            }
            return View(new List<Role>());
        }

        public IActionResult Create()
        {
            return View(new Role());
        }

        [HttpPost]
        public async Task<IActionResult> Save(Role model)
        {
            await _roleService.CreateRoleAsync(model.Name);
            return new NoContentResult();
        }

        public async Task<IActionResult> Edit(string id)
        {
            var model = await _roleManager.FindByIdAsync(id);
            if (model == null)
                return NotFound();

            return View(model);
        }

        public async Task<IActionResult> Update(Role model)
        {
            await _roleService.UpdateRoleNameAsync(model.Id, model.Name);
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
