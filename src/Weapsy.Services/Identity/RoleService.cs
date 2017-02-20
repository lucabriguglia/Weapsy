using Microsoft.AspNetCore.Identity;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Weapsy.Data.Entities;
using Weapsy.Infrastructure.Identity;

namespace Weapsy.Services.Identity
{
    public class RoleService : IRoleService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public RoleService(UserManager<User> userManager,
            RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task CreateRoleAsync(string name)
        {
            var role = new Role {Name = name};

            var result = await _roleManager.CreateAsync(role);

            if (!result.Succeeded)
                throw new Exception(GetErrorMessage(result));
        }

        public async Task UpdateRoleNameAsync(Guid id, string name)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());

            if (role == null)
                throw new Exception("Role not found.");

            role.Name = name;

            var result = await _roleManager.UpdateAsync(role);

            if (!result.Succeeded)
                throw new Exception(GetErrorMessage(result));
        }

        public async Task DeleteRoleAsync(Guid id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());

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

        public async Task<IList<Guid>> GetDefaultPageViewPermissionRoleIdsAsync()
        {
            var result = new List<Guid>();

            //var adminRole = await _roleManager.FindByNameAsync(Administrator.Name);
            //if (adminRole != null)
            //    result.Add(adminRole.Id);

            //result.Add(((int)DefaultRoles.Everyone).ToString());

            result.Add(Administrator.Id);

            return result;
        }

        public async Task<IList<Guid>> GetDefaultPageEditPermissionRoleIdsAsync()
        {
            var result = new List<Guid>();

            //var adminRole = await _roleManager.FindByNameAsync(Administrator.Name);
            //if (adminRole != null)
            //    result.Add(adminRole.Id);

            result.Add(Administrator.Id);

            return result;
        }

        public async Task<IList<Guid>> GetDefaultModuleViewPermissionRoleIdsAsync()
        {
            var result = new List<Guid>();

            //var adminRole = await _roleManager.FindByNameAsync(Administrator.Name);
            //if (adminRole != null)
            //    result.Add(adminRole.Id);

            //result.Add(((int)DefaultRoles.Everyone).ToString());

            result.Add(Administrator.Id);

            return result;
        }

        public async Task<IList<Guid>> GetDefaultModuleEditPermissionRoleIdsAsync()
        {
            var result = new List<Guid>();

            //var adminRole = await _roleManager.FindByNameAsync(Administrator.Name);
            //if (adminRole != null)
            //    result.Add(adminRole.Id);

            result.Add(Administrator.Id);

            return result;
        }

        private string GetErrorMessage(IdentityResult result)
        {
            var builder = new StringBuilder();

            foreach (var error in result.Errors)
                builder.AppendLine(error.Description);

            return builder.ToString();
        }

        public IList<Role> GetAllRoles()
        {
            var result = _roleManager.Roles.ToList();

            result.Add(new Role
            {
                Id = Everyone.Id,
                Name = Everyone.Name
            });

            result.Add(new Role
            {
                Id = Registered.Id,
                Name = Registered.Name
            });

            result.Add(new Role
            {
                Id = Anonymous.Id,
                Name = Anonymous.Name
            });

            return result.OrderBy(x => x.Name).ToList();
        }

        public IList<Role> GetRolesFromIds(IEnumerable<Guid> roleIds)
        {
            var result = new List<Role>();

            foreach (var roleId in roleIds)
            {
                if (Everyone.Id == roleId)
                {
                    result.Add(new Role
                    {
                        Id = Everyone.Id,
                        Name = Everyone.Name
                    });
                }

                if (Registered.Id == roleId)
                {
                    result.Add(new Role
                    {
                        Id = Registered.Id,
                        Name = Registered.Name
                    });
                }

                if (Anonymous.Id == roleId)
                {
                    result.Add(new Role
                    {
                        Id = Anonymous.Id,
                        Name = Anonymous.Name
                    });
                }

                var role = _roleManager.FindByIdAsync(roleId.ToString()).Result;

                if (role != null)
                {
                    result.Add(role);
                }
            }

            return result;
        }
    }
}
