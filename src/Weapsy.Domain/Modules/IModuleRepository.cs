using System;
using System.Collections.Generic;
using Weapsy.Infrastructure.Domain;

namespace Weapsy.Domain.Modules
{
    public interface IModuleRepository : IRepository<Module>
    {
        Module GetById(Guid id);
        Module GetById(Guid siteId, Guid id);
        ICollection<Module> GetAll();
        int GetCountByModuleTypeId(Guid moduleTypeId);
        int GetCountByModuleId(Guid moduleId);
        void Create(Module module);
        void Update(Module module);
    }
}