using System;
using Weapsy.Core.Domain;

namespace Weapsy.Domain.Model.Roles.Rules
{
    public interface IRoleRules : IRules<Role>
    {
        bool IsRoleIdUnique(Guid id);
        bool IsRoleNameUnique(string name);
    }
}
