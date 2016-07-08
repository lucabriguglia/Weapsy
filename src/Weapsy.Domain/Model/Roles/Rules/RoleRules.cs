using System;

namespace Weapsy.Domain.Model.Roles.Rules
{
    public class RoleRules : IRoleRules
    {
        private readonly IRoleRepository _roleRepository;

        public RoleRules(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public bool IsRoleIdUnique(Guid id)
        {
            return _roleRepository.GetById(id) == null;
        }

        public bool IsRoleNameUnique(string name)
        {
            return _roleRepository.GetByName(name) == null;
        }
    }
}