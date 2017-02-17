using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Threading.Tasks;
using Weapsy.Data.Entities;

namespace Weapsy.Data.Identity
{
    public interface IUserService
    {
        UsersViewModel GetUsersViewModel(UsersQuery query);
        Task<UserRolesViewModel> GetUserRolesViewModelAsync(Guid id);
        bool IsUserAuthorized(IPrincipal user, IEnumerable<Role> roles);
        bool IsUserAuthorized(IPrincipal user, IEnumerable<string> roleNames);
        Task CreateUserAsync(string email);
        Task AddUserToRoleAsync(Guid id, string roleName);
        Task RemoveUserFromRoleAsync(Guid id, string roleName);
        Task DeleteUserAsync(Guid id);
    }
}
