using System.Collections.Generic;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Weapsy.Services.Identity
{
    public interface IUserService
    {
        UsersViewModel GetUsersViewModel(UsersQuery query);
        Task<UserRolesViewModel> GetUserRolesViewModelAsync(string id);
        bool IsUserAuthorized(IPrincipal user, IEnumerable<IdentityRole> roles);
        bool IsUserAuthorized(IPrincipal user, IEnumerable<string> roles);
        Task CreateUserAsync(string email);
        Task AddUserToRoleAsync(string id, string roleName);
        Task RemoveUserFromRoleAsync(string id, string roleName);
        Task DeleteUserAsync(string id);
    }
}
