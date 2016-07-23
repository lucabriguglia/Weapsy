using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Weapsy.Mvc.Context;
using Weapsy.Mvc.Controllers;
using System.Linq;
using Weapsy.Services.Identity;

namespace Weapsy.Api
{
    [Route("api/[controller]")]
    public class RoleController : BaseAdminController
    {
        private readonly IIdentityService _identityService;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleController(IIdentityService identityService,
            RoleManager<IdentityRole> roleManager,
            IContextService contextService)
            : base(contextService)
        {
            _identityService = identityService;
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
        public async Task<IActionResult> Post([FromBody]IdentityRole model)
        {
            await _identityService.CreateRole(model.Name);
            return Ok(string.Empty);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody]IdentityRole model)
        {
            await _identityService.UpdateRoleName(model.Id, model.Name);
            return Ok(string.Empty);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _identityService.DeleteRole(id);
            return Ok(string.Empty);
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
