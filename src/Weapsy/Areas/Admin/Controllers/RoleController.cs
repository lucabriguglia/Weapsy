using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Weapsy.Core.Dispatcher;
using Weapsy.Mvc.Context;
using Weapsy.Mvc.Controllers;

namespace Weapsy.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RoleController : BaseAdminController
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ICommandSender _commandSender;

        public RoleController(RoleManager<IdentityRole> roleManager,
            ICommandSender commandSender,
            IContextService contextService)
            : base(contextService)
        {
            _roleManager = roleManager;
            _commandSender = commandSender;
        }

        public async Task<IActionResult> Index()
        {
            var model = _roleManager.Roles.ToList();
            return View(model);
        }

        public async Task<IActionResult> Create()
        {
            return View(new IdentityRole());
        }

        public async Task<IActionResult> Edit(string id)
        {
            var model = await _roleManager.FindByIdAsync(id);
            if (model == null) return NotFound();
            return View(model);
        }
    }
}
