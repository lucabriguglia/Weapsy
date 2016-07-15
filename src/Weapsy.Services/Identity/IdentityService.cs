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

        public async Task UpdateRoleName(string id, string name)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
                throw new Exception("Role not found.");

            role.Name = name;

            var result = await _roleManager.UpdateAsync(role);

            if (!result.Succeeded)
                throw new Exception(GetErrorMessage(result));
        }

        public async Task DeleteRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
                throw new Exception("Role not found.");

            var result = await _roleManager.DeleteAsync(role);

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
