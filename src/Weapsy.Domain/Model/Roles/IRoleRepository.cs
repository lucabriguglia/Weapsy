using System;
using System.Collections.Generic;
using Weapsy.Core.Domain;

namespace Weapsy.Domain.Model.Roles
{
    public interface IRoleRepository : IRepository<Role>
    {
        Role GetById(Guid id);
        Role GetByName(string name);
        IEnumerable<Role> GetAll();
        void Create(Role role);
        void Update(Role role);
    }
}
