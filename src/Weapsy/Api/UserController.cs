using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Weapsy.Data.Entities;
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

        public UserController(IQueryDispatcher queryDispatcher,
            UserManager<User> userManager,
            IContextService contextService)
            : base(contextService)
        {
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
        public IActionResult AddToRole(Guid id, [FromBody]string roleName)
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        [Route("{id}/remove-from-role")]
        public IActionResult RemoveFromRole(Guid id, [FromBody]string roleName)
        {
            throw new NotImplementedException();
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
