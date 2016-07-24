using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Weapsy.Services.Identity
{
    public interface IUserService
    {
        Task<UserRolesViewModel> GetUserRolesViewModel(string id);
        bool IsUserAuthorized(ClaimsPrincipal user, IEnumerable<string> roles);
    }
}
