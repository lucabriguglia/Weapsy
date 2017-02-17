using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Weapsy.Data.Entities;

namespace Weapsy.Data.Identity
{
    public interface IRoleService
    {
        Task CreateRoleAsync(string name);
        Task UpdateRoleNameAsync(Guid id, string name);
        Task DeleteRoleAsync(Guid id);
        Task<IList<Guid>> GetDefaultPageViewPermissionRoleIdsAsync();
        Task<IList<Guid>> GetDefaultPageEditPermissionRoleIdsAsync();
        Task<IList<Guid>> GetDefaultModuleViewPermissionRoleIdsAsync();
        Task<IList<Guid>> GetDefaultModuleEditPermissionRoleIdsAsync();
        IList<Role> GetAllRoles();
        IList<Role> GetRolesFromIds(IEnumerable<Guid> roleIds);
    }
}
