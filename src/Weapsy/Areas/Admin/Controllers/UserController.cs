using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Weapsy.Data.Entities;
using Weapsy.Framework.Queries;
using Weapsy.Mvc.Context;
using Weapsy.Mvc.Controllers;
using Weapsy.Reporting.Users.Queries;
using Weapsy.Reporting.Users;

namespace Weapsy.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : BaseAdminController
    {
        private readonly IQueryDispatcher _queryDispatcher;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public UserController(IQueryDispatcher queryDispatcher,
            UserManager<User> userManager,
            RoleManager<Role> roleManager,
            IContextService contextService)
            : base(contextService)
        {
            _queryDispatcher = queryDispatcher;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Create()
        {
            var model = await _queryDispatcher.DispatchAsync<GetDefaultUserAdminModel, UserAdminModel>(new GetDefaultUserAdminModel());
            return View(model);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var query = new GetUserAdminModel
            {
                Id = id
            };

            var model = await _queryDispatcher.DispatchAsync<GetUserAdminModel, UserAdminModel>(query);

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

            var model = await _queryDispatcher.DispatchAsync<GetUserRolesViewModel, UserRolesViewModel>(query);

            if (model == null)
                return NotFound();

            return View(model);
        }
    }
}
