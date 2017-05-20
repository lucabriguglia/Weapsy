using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Weapsy.Domain.Users;
using Weapsy.Domain.Users.Commands;
using Weapsy.Framework.Commands;
using Weapsy.Framework.Queries;
using Weapsy.Mvc.Context;
using Weapsy.Mvc.Controllers;
using Weapsy.Reporting.Users;
using Weapsy.Reporting.Users.Queries;

namespace Weapsy.Api
{
    [Route("api/[controller]")]
    public class UserController : BaseAdminController
    {
        private readonly ICommandSender _commandSender;
        private readonly IQueryDispatcher _queryDispatcher;
        private readonly UserManager<Data.Entities.User> _userManager;

        public UserController(ICommandSender commandSender,
            IQueryDispatcher queryDispatcher,
            UserManager<Data.Entities.User> userManager,
            IContextService contextService)
            : base(contextService)
        {
            _commandSender = commandSender;
            _queryDispatcher = queryDispatcher;
            _userManager = userManager;
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
        public IActionResult Post(string email)
        {
            throw new NotImplementedException();
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
            await _commandSender.SendAsync<AddUserToRole, User>(new AddUserToRole { Id = id, RoleName = roleName });
            return new NoContentResult();
        }

        [HttpPut]
        [Route("{id}/remove-from-role")]
        public async Task<IActionResult> RemoveFromRole(Guid id, [FromBody]string roleName)
        {
            await _commandSender.SendAsync<RemoveUserFromRole, User>(new RemoveUserFromRole { Id = id, RoleName = roleName });
            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            throw new NotImplementedException();
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
