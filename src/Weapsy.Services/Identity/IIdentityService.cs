using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Weapsy.Services.Identity
{
    public interface IIdentityService
    {
        Task CreateRole(string name);
        Task UpdateRoleName(string id, string name);
        Task DeleteRole(string id);
        bool IsUserAuthorized(ClaimsPrincipal user, IEnumerable<string> roles);
    }
}
