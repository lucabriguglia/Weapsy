using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Weapsy.Cqrs;
using Weapsy.Data.TempIdentity;
using Weapsy.Domain.Users;
using Weapsy.Domain.Users.Commands;
using Weapsy.Mvc.Context;
using Weapsy.Mvc.Controllers;
using Weapsy.Reporting.Users;
using Weapsy.Reporting.Users.Queries;

namespace Weapsy.Web.Api
{
    [Route("api/[controller]")]
    public class UserController : BaseAdminController
    {
        private readonly IDispatcher _dispatcher;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(IDispatcher dispatcher,
            UserManager<ApplicationUser> userManager,
            IContextService contextService)
            : base(contextService)
        {
            _dispatcher = dispatcher;
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
            await _dispatcher.SendAndPublishAsync<AddUserToRole, User>(new AddUserToRole { Id = id, RoleName = roleName });
            return new NoContentResult();
        }

        [HttpPut]
        [Route("{id}/remove-from-role")]
        public async Task<IActionResult> RemoveFromRole(Guid id, [FromBody]string roleName)
        {
            await _dispatcher.SendAndPublishAsync<RemoveUserFromRole, User>(new RemoveUserFromRole { Id = id, RoleName = roleName });
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

            var model = await _dispatcher.GetResultAsync<GetUsersAdminViewModel, UsersAdminViewModel>(query);

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

            var model = await _dispatcher.GetResultAsync<GetUserAdminModel, UserAdminModel>(query);

            if (model == null)
                return NotFound();

            return Ok(model);
        }
    }
}
