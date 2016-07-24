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
        Task<IList<string>> GetDefaultModuleViewPermissionRoleIds();
    }
}
