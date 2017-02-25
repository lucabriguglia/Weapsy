using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Weapsy.Data.Entities;
using Weapsy.Data.Identity;
using Weapsy.Mvc.Context;
using Weapsy.Mvc.Controllers;
using Weapsy.Infrastructure.Queries;
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
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;

        public UserController(IQueryDispatcher queryDispatcher,
            UserManager<User> userManager,
            RoleManager<Role> roleManager,
            IUserService userService,
            IRoleService roleService,
            IContextService contextService)
            : base(contextService)
        {
            _queryDispatcher = queryDispatcher;
            _userManager = userManager;
            _roleManager = roleManager;
            _userService = userService;
            _roleService = roleService;
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
