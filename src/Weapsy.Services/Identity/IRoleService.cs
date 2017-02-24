using System;
using System.Collections.Generic;
using Weapsy.Data.Entities;

namespace Weapsy.Services.Identity
{
    public interface IRoleService
    {
        IList<Role> GetAllRoles();
        IList<Role> GetRolesFromIds(IEnumerable<Guid> roleIds);
    }
}
