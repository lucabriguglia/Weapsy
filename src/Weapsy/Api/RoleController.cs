using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Weapsy.Mvc.Context;
using Weapsy.Mvc.Controllers;
using System.Linq;

namespace Weapsy.Api
{
    [Route("api/[controller]")]
    public class RoleController : BaseAdminController
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleController(RoleManager<IdentityRole> roleManager,
            IContextService contextService)
            : base(contextService)
        {
            _roleManager = roleManager;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            if (_roleManager.SupportsQueryableRoles)
            {
                var roles = _roleManager.Roles.ToList();
                return Ok(roles);
            }
            return Ok(string.Empty);
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
        [Route("{name}")]
        public async Task<IActionResult> Post(string name)
        {
            var role = new IdentityRole(name);

            var result = await _roleManager.CreateAsync(role);
            if (result.Succeeded)
                return Ok(string.Empty);

            throw new Exception(GetErrors(result));
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody]IdentityRole model)
        {
            var result = await _roleManager.UpdateAsync(model);
            if (result.Succeeded)
                return Ok(string.Empty);

            throw new Exception(GetErrors(result));
        }

        [HttpDelete]
        [Route("{name}")]
        public async Task<IActionResult> Delete(string name)
        {
            var role = await _roleManager.FindByNameAsync(name);
            if (role == null)
                return NotFound();

            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
                return Ok(string.Empty);

            throw new Exception(GetErrors(result));
        }

        [HttpGet("{name}")]
        [Route("isNameUnique")]
        public async Task<IActionResult> IsNameUnique(string name)
        {
            var isNameUnique = await _roleManager.FindByNameAsync(name) == null;
            return Ok(isNameUnique);
        }

        private string GetErrors(IdentityResult result)
        {
            var builder = new StringBuilder();
            foreach (var error in result.Errors)
            {
                builder.AppendLine(error.Description);
            }
            return builder.ToString();
        }
    }
}
