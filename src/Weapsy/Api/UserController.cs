using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Weapsy.Data.Entities;
using Weapsy.Data.Identity;
using Weapsy.Infrastructure.Queries;
using Weapsy.Mvc.Context;
using Weapsy.Mvc.Controllers;
using Weapsy.Reporting.Users;
using Weapsy.Reporting.Users.Queries;

namespace Weapsy.Api
{
    [Route("api/[controller]")]
    public class UserController : BaseAdminController
    {
        private readonly IQueryDispatcher _queryDispatcher;
        private readonly UserManager<User> _userManager;
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;

        public UserController(IQueryDispatcher queryDispatcher,
            UserManager<User> userManager,
            IUserService userService,
            IRoleService roleService,
            IContextService contextService)
            : base(contextService)
        {
            _queryDispatcher = queryDispatcher;
            _userManager = userManager;
            _userService = userService;
            _roleService = roleService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            throw new NotImplementedException();
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<IActionResult> Post(string email)
        {
            await _userService.CreateUserAsync(email);
            return new NoContentResult();
        }

        [HttpPut]
        [Route("{id}/change-email")]
        public IActionResult ChangeEmail([FromBody]string email)
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        [Route("{id}/add-to-role")]
        public async Task<IActionResult> AddToRole(Guid id, [FromBody]string roleName)
        {
            await _userService.AddUserToRoleAsync(id, roleName);
            return new NoContentResult();
        }

        [HttpPut]
        [Route("{id}/remove-from-role")]
        public async Task<IActionResult> RemoveFromRole(Guid id, [FromBody]string roleName)
        {
            await _userService.RemoveUserFromRoleAsync(id, roleName);
            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _userService.DeleteUserAsync(id);
            return new NoContentResult();
        }

        [HttpGet("{email}")]
        [Route("isEmailUnique")]
        public async Task<IActionResult> IsEmailUnique(string email)
        {
            var isEmailUnique = await _userManager.FindByEmailAsync(email) == null;
            return Ok(isEmailUnique);
        }

        [HttpGet]
        [Route("admin-list")]
        public async Task<IActionResult> AdminList(int startIndex, int numberOfUsers)
        {
            var query = new GetUsersAdminViewModel
            {
                StartIndex = startIndex,
                NumberOfUsers = numberOfUsers
            };

            var model = await _queryDispatcher.DispatchAsync<GetUsersAdminViewModel, UsersAdminViewModel>(query);

            return Ok(model);
        }

        [HttpGet]
        [Route("{id}/admin-edit")]
        public async Task<IActionResult> AdminEdit(Guid id)
        {
            var query = new GetUserAdminModel
            {
                Id = id
            };

            var model = await _queryDispatcher.DispatchAsync<GetUserAdminModel, UserAdminModel>(query);

            if (model == null)
                return NotFound();

            return Ok(model);
        }
    }
}
