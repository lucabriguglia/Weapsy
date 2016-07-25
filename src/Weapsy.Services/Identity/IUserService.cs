using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Weapsy.Services.Identity
{
    public interface IUserService
    {
        UsersViewModel GetUsersViewModel(UsersQuery query);
        Task<UserRolesViewModel> GetUserRolesViewModel(string id);
        bool IsUserAuthorized(ClaimsPrincipal user, IEnumerable<string> roles);
        Task CreateUser(string email);
        Task AddUserToRole(string id, string roleName);
        Task RemoveUserFromRole(string id, string roleName);
        Task DeleteUser(string id);
    }
}
