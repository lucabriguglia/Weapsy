using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Claims;
using Weapsy.Core.Identity;
using System.Linq;

namespace Weapsy.Services.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public IdentityService(UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
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

        public async Task<UserRolesViewModel> GetUserRolesViewModel(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return null;

            var userRoles = await _userManager.GetRolesAsync(user);
            var availableRoles = _roleManager.Roles.Where(x => !userRoles.Contains(x.Name)).ToList();

            var model = new UserRolesViewModel
            {
                User = user,
                AvailableRoles = availableRoles.OrderBy(x => x.Name).ToList(),
                UserRoles = userRoles.OrderBy(x => x).ToList()
            };

            return model;
        }

        public bool IsUserAuthorized(ClaimsPrincipal user, IEnumerable<string> roles)
        {
            if (user == null || roles == null)
                return false;

            foreach (var role in roles)
            {
                if (role == Roles.Everyone.ToString())
                    return true;

                if (role == Roles.Registered.ToString() && user.Identity.IsAuthenticated)
                    return true;

                if (role == Roles.Anonymous.ToString() && !user.Identity.IsAuthenticated)
                    return true;

                if (user.IsInRole(role))
                    return true;
            }

            return false;
        }

        public async Task<IList<string>> GetDefaultPageViewPermissionRoleIds()
        {
            var result = new List<string>();

            var adminRole = await _roleManager.FindByNameAsync(DefaultRoleNames.Administrator);
            if (adminRole != null)
                result.Add(adminRole.Id);

            return result;
        }

        public async Task<IList<string>> GetDefaultModuleViewPermissionRoleIds()
        {
            var result = new List<string>();

            var adminRole = await _roleManager.FindByNameAsync(DefaultRoleNames.Administrator);
            if (adminRole != null)
                result.Add(adminRole.Id);

            return result;
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
