using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Weapsy.Data.Entities;
using Weapsy.Infrastructure.Identity;

namespace Weapsy.Data.Identity
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
