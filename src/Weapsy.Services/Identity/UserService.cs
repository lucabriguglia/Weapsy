using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Claims;
using Weapsy.Core.Identity;
using System.Linq;
using System;

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

        public async Task<UsersViewModel> GetUsersViewModel(UsersQuery query)
        {
            var totalRecords = _userManager.Users.Count();

            var q = _userManager.Users
                .Skip(query.StartIndex)
                .Take(query.NumberOfUsers)
                .OrderBy(x => x.Email);

            var viewModel = new UsersViewModel
            {
                Users = q.ToList(),
                TotalRecords = totalRecords,
                NumberOfPages = (int)Math.Ceiling((double)totalRecords / query.NumberOfUsers)
            };

            return viewModel;
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

        private string GetErrorMessage(IdentityResult result)
        {
            var builder = new StringBuilder();

            foreach (var error in result.Errors)
                builder.AppendLine(error.Description);

            return builder.ToString();
        }
    }
}
