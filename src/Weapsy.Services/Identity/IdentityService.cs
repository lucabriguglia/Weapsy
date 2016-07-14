using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Weapsy.Services.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public IdentityService(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task CreateRole(string name)
        {
            var role = new IdentityRole(name);

            var result = await _roleManager.CreateAsync(role);

            if (!result.Succeeded)
                throw new Exception(GetErrorMessage(result));
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
