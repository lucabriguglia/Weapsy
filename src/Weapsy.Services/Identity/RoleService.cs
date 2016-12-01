using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Weapsy.Infrastructure.Identity;

namespace Weapsy.Services.Identity
{
    public class RoleService : IRoleService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleService(UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task CreateRoleAsync(string name)
        {
            var role = new IdentityRole(name);

            var result = await _roleManager.CreateAsync(role);

            if (!result.Succeeded)
                throw new Exception(GetErrorMessage(result));
        }

        public async Task UpdateRoleNameAsync(string id, string name)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
                throw new Exception("Role not found.");

            role.Name = name;

            var result = await _roleManager.UpdateAsync(role);

            if (!result.Succeeded)
                throw new Exception(GetErrorMessage(result));
        }

        public async Task DeleteRoleAsync(string id)
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

        public async Task<IList<string>> GetDefaultPageViewPermissionRoleIdsAsync()
        {
            var result = new List<string>();

            var adminRole = await _roleManager.FindByNameAsync(DefaultRoleNames.Administrator);
            if (adminRole != null)
                result.Add(adminRole.Id);

            //result.Add(((int)DefaultRoles.Everyone).ToString());

            return result;
        }

        public async Task<IList<string>> GetDefaultPageEditPermissionRoleIdsAsync()
        {
            var result = new List<string>();

            var adminRole = await _roleManager.FindByNameAsync(DefaultRoleNames.Administrator);
            if (adminRole != null)
                result.Add(adminRole.Id);

            return result;
        }

        public async Task<IList<string>> GetDefaultModuleViewPermissionRoleIdsAsync()
        {
            var result = new List<string>();

            var adminRole = await _roleManager.FindByNameAsync(DefaultRoleNames.Administrator);
            if (adminRole != null)
                result.Add(adminRole.Id);

            //result.Add(((int)DefaultRoles.Everyone).ToString());

            return result;
        }

        public async Task<IList<string>> GetDefaultModuleEditPermissionRoleIdsAsync()
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

        public IList<IdentityRole> GetAllRoles()
        {
            var result = _roleManager.Roles.ToList();

            var defaultRoles = Enum.GetValues(typeof(DefaultRoles)).Cast<DefaultRoles>();

            foreach (var role in defaultRoles)
                result.Add(new IdentityRole
                {
                    Id = ((int)role).ToString(),
                    Name = role.ToString()
                });

            return result.OrderBy(x => x.Name).ToList();
        }

        public IList<IdentityRole> GetRolesFromIds(IEnumerable<string> roleIds)
        {
            var result = new List<IdentityRole>();

            foreach (var roleId in roleIds)
            {
                int id;

                if (int.TryParse(roleId, out id))
                {
                    if (Enum.IsDefined(typeof(DefaultRoles), id))
                    {
                        result.Add(new IdentityRole
                        {
                            Id = id.ToString(),
                            Name = Enum.GetName(typeof(DefaultRoles), id)
                        });
                        continue;
                    }
                }

                var role = _roleManager.FindByIdAsync(roleId).Result;
                if (role != null)
                    result.Add(role);
            }

            return result;
        }
    }
}
