using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Security.Principal;
using Weapsy.Infrastructure.Identity;

namespace Weapsy.Services.Identity
{
    public class UserService : IUserService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserService(UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public UsersViewModel GetUsersViewModel(UsersQuery query)
        {
            if (!_userManager.SupportsQueryableUsers)
            {
                return new UsersViewModel
                {
                    Users = new List<IdentityUser>(),
                    TotalRecords = 0,
                    NumberOfPages = 0
                };
            }

            var totalRecords = _userManager.Users.Count();

            var q = _userManager.Users
                .OrderBy(x => x.Email)
                .Skip(query.StartIndex);

            // Part-1: If 0 (zero) is passed for query.NumberOfUsers, we load all users

            if (query.NumberOfUsers > 0)
                q = q.Take(query.NumberOfUsers);

            var viewModel = new UsersViewModel
            {
                Users = q.ToList(),
                TotalRecords = totalRecords,
                NumberOfPages = (int)Math.Ceiling((double)totalRecords / query.NumberOfUsers)
            };

            return viewModel;
        }

        public async Task<UserRolesViewModel> GetUserRolesViewModelAsync(string id)
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

        public bool IsUserAuthorized(IPrincipal user, IEnumerable<IdentityRole> roles)
        {
            return IsUserAuthorized(user, roles.Select(x => x.Name));
        }

        public bool IsUserAuthorized(IPrincipal user, IEnumerable<string> roles)
        {
            if (user == null || roles == null || !roles.Any())
                return false;

            foreach (var role in roles)
            {
                if (role == DefaultRoles.Everyone.ToString())
                    return true;

                if (role == DefaultRoles.Registered.ToString() && user.Identity.IsAuthenticated)
                    return true;

                if (role == DefaultRoles.Anonymous.ToString() && !user.Identity.IsAuthenticated)
                    return true;

                if (user.IsInRole(role))
                    return true;
            }

            return false;
        }

        public async Task CreateUserAsync(string email)
        {
            var user = new IdentityUser { UserName = email, Email = email };

            var result = await _userManager.CreateAsync(user);

            if (!result.Succeeded)
                throw new Exception(GetErrorMessage(result));
        }

        public async Task AddUserToRoleAsync(string id, string roleName)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                throw new Exception("User Not Found.");

            var result = await _userManager.AddToRoleAsync(user, roleName);
            if (!result.Succeeded)
                throw new Exception(GetErrorMessage(result));
        }

        public async Task RemoveUserFromRoleAsync(string id, string roleName)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                throw new Exception("User Not Found.");

            var result = await _userManager.RemoveFromRoleAsync(user, roleName);
            if (!result.Succeeded)
                throw new Exception(GetErrorMessage(result));
        }

        public async Task DeleteUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                throw new Exception("User Not Found.");

            var result = await _userManager.DeleteAsync(user);
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
