using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Weapsy.Mvc.Context;
using Weapsy.Mvc.Controllers;
using Weapsy.Services.Identity;

namespace Weapsy.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RoleController : BaseAdminController
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IIdentityService _identityService;

        public RoleController(RoleManager<IdentityRole> roleManager,
            IIdentityService identityService,
            IContextService contextService)
            : base(contextService)
        {
            _roleManager = roleManager;
            _identityService = identityService;
        }

        public IActionResult Index()
        {
            if (_roleManager.SupportsQueryableRoles)
            {
                var model = _roleManager.Roles.OrderBy(x => x.Name).ToList();
                return View(model);
            }
            return View(new List<IdentityRole>());
        }

        public IActionResult Create()
        {
            return View(new IdentityRole());
        }

        [HttpPost]
        public async Task<IActionResult> Save(IdentityRole model)
        {
            await _identityService.CreateRole(model.Name);
            return Ok(string.Empty);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var model = await _roleManager.FindByIdAsync(id);
            if (model == null)
                return NotFound();

            return View(model);
        }

        public async Task<IActionResult> Update(IdentityRole model)
        {
            await _identityService.UpdateRoleName(model.Id, model.Name);
            return Ok(string.Empty);
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
