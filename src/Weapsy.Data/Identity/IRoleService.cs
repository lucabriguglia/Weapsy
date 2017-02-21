using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Weapsy.Data.Entities;

namespace Weapsy.Data.Identity
{
    public interface IRoleService
    {
        IList<Role> GetAllRoles();
        IList<Role> GetRolesFromIds(IEnumerable<Guid> roleIds);
    }
}
