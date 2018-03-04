using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Weapsy.Cqrs;
using Weapsy.Data.TempIdentity;
using Weapsy.Mvc.Context;
using Weapsy.Mvc.Controllers;
using Weapsy.Reporting.Users;
using Weapsy.Reporting.Users.Queries;

namespace Weapsy.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : BaseAdminController
    {
        private readonly IDispatcher _dispatcher;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public UserController(IDispatcher dispatcher,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IContextService contextService)
            : base(contextService)
        {
            _dispatcher = dispatcher;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Create()
        {
            var model = await _dispatcher.GetResultAsync<GetDefaultUserAdminModel, UserAdminModel>(new GetDefaultUserAdminModel());
            return View(model);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var query = new GetUserAdminModel
            {
                Id = id
            };

            var model = await _dispatcher.GetResultAsync<GetUserAdminModel, UserAdminModel>(query);

            if (model == null)
                return NotFound();

            return View(model);
        }

        public async Task<IActionResult> Roles(Guid id)
        {
            var query = new GetUserRolesViewModel
            {
                Id = id
            };

            var model = await _dispatcher.GetResultAsync<GetUserRolesViewModel, UserRolesViewModel>(query);

            if (model == null)
                return NotFound();

            return View(model);
        }
    }
}
