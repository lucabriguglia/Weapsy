using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Weapsy.Mvc.Context;
using Weapsy.Mvc.Controllers;
using System.Linq;
using Weapsy.Data.Entities;
using System.Text;

namespace Weapsy.Api
{
    [Route("api/[controller]")]
    public class RoleController : BaseAdminController
    {
        private readonly RoleManager<Role> _roleManager;

        public RoleController(RoleManager<Role> roleManager,
            IContextService contextService)
            : base(contextService)
        {
            _roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult Get()
        {
            if (_roleManager.SupportsQueryableRoles)
            {
                var roles = _roleManager.Roles.OrderBy(x => x.Name).ToList();
                return Ok(roles);
            }
            return new NoContentResult();
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
                return NotFound();

            return Ok(role);
        }

        [HttpGet]
        [Route("{name}/by-name")]
        public async Task<IActionResult> GetByName(string name)
        {
            var role = await _roleManager.FindByNameAsync(name);

            if (role == null)
                return NotFound();

            return Ok(role);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Role role)
        {
            var result = await _roleManager.CreateAsync(role);

            if (!result.Succeeded)
                throw new Exception(GetErrorMessage(result));

            return new NoContentResult();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody]Role model)
        {
            var role = await _roleManager.FindByIdAsync(model.Id.ToString());

            if (role == null)
                throw new Exception("Role not found.");

            role.Name = model.Name;

            var result = await _roleManager.UpdateAsync(role);

            if (!result.Succeeded)
                throw new Exception(GetErrorMessage(result));

            return new NoContentResult();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());

            if (role == null)
                throw new Exception("Role not found.");

            var result = await _roleManager.DeleteAsync(role);

            if (!result.Succeeded)
                throw new Exception(GetErrorMessage(result));

            return new NoContentResult();
        }

        [HttpGet("{name}")]
        [Route("isRoleNameUnique")]
        public async Task<IActionResult> IsRoleNameUnique(string name)
        {
            var isNameUnique = await _roleManager.FindByNameAsync(name) == null;
            return Ok(isNameUnique);
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
