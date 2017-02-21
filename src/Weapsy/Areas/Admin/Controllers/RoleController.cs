using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Weapsy.Data.Entities;
using Weapsy.Mvc.Context;
using Weapsy.Mvc.Controllers;

namespace Weapsy.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RoleController : BaseAdminController
    {
        private readonly RoleManager<Role> _roleManager;

        public RoleController(RoleManager<Role> roleManager,
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
            return View(new List<Role>());
        }

        public IActionResult Create()
        {
            return View(new Role());
        }

        public async Task<IActionResult> Edit(string id)
        {
            var model = await _roleManager.FindByIdAsync(id);

            if (model == null)
                return NotFound();

            return View(model);
        }
    }
}
