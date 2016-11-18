using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Weapsy.Services.Identity
{
    public interface IRoleService
    {
        Task CreateRole(string name);
        Task UpdateRoleName(string id, string name);
        Task DeleteRole(string id);
        Task<IList<string>> GetDefaultPageViewPermissionRoleIds();
        Task<IList<string>> GetDefaultPageEditPermissionRoleIds();
        Task<IList<string>> GetDefaultModuleViewPermissionRoleIds();
        Task<IList<string>> GetDefaultModuleEditPermissionRoleIds();
        IList<IdentityRole> GetAllRoles();
        IList<IdentityRole> GetRolesFromIds(IEnumerable<string> roleIds);
    }
}
