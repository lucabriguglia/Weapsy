using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Weapsy.Services.Identity
{
    public interface IRoleService
    {
        Task CreateRoleAsync(string name);
        Task UpdateRoleNameAsync(string id, string name);
        Task DeleteRoleAsync(string id);
        Task<IList<string>> GetDefaultPageViewPermissionRoleIdsAsync();
        Task<IList<string>> GetDefaultPageEditPermissionRoleIdsAsync();
        Task<IList<string>> GetDefaultModuleViewPermissionRoleIdsAsync();
        Task<IList<string>> GetDefaultModuleEditPermissionRoleIdsAsync();
        IList<IdentityRole> GetAllRoles();
        IList<IdentityRole> GetRolesFromIds(IEnumerable<string> roleIds);
    }
}
