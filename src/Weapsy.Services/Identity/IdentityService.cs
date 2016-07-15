using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Claims;
using Weapsy.Core.Identity;

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

        public bool IsUserAuthorized(ClaimsPrincipal user, IEnumerable<string> roles)
        {
            foreach (var role in roles)
            {
                if (role == Roles.Everyone.ToString())
                    return true;

                if (role == Roles.Registered.ToString() && user.Identity.IsAuthenticated)
                    return true;

                if (role == Roles.Anonymous.ToString() && !user.Identity.IsAuthenticated)
                    return true;

                return user.IsInRole(role);
            }
            return false;
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
