using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Weapsy.Mvc.Context;
using Weapsy.Mvc.Controllers;

namespace Weapsy.Api
{
    [Route("api/[controller]")]
    public class UserController : BaseAdminController
    {
        private readonly UserManager<IdentityUser> _userManager;

        public UserController(UserManager<IdentityUser> userManager,
            IContextService contextService)
            : base(contextService)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Get()
        {
            if (_userManager.SupportsQueryableUsers)
                return Ok(_userManager.Users.ToList());

            return Ok(string.Empty);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> Post(string email)
        {
            var user = new IdentityUser { UserName = email, Email = email };
            var result = await _userManager.CreateAsync(user);

            if (result.Succeeded)
                return Ok(string.Empty);

            throw new Exception(GetErrorMessage(result));
        }

        [HttpPut]
        [Route("{id}/change-email")]
        public async Task<IActionResult> ChangeEmail([FromBody]string email)
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        [Route("{id}/add-to-role")]
        public async Task<IActionResult> AddToRole(string id, [FromBody]string roleName)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            var result = await _userManager.AddToRoleAsync(user, roleName);
            if (result.Succeeded)
                return Ok(string.Empty);

            throw new Exception(GetErrorMessage(result));
        }

        [HttpPut]
        [Route("{id}/remove-from-role")]
        public async Task<IActionResult> RemoveFromRole(string id, [FromBody]string roleName)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            var result = await _userManager.RemoveFromRoleAsync(user, roleName);
            if (result.Succeeded)
                return Ok(string.Empty);

            throw new Exception(GetErrorMessage(result));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
                return Ok(string.Empty);

            throw new Exception(GetErrorMessage(result));
        }

        [HttpGet("{email}")]
        [Route("isEmailUnique")]
        public async Task<IActionResult> IsEmailUnique(string email)
        {
            var isEmailUnique = await _userManager.FindByEmailAsync(email) == null;
            return Ok(isEmailUnique);
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
