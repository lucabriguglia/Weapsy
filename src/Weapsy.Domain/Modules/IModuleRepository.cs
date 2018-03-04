using System;

namespace Weapsy.Domain.Modules
{
    public interface IModuleRepository
    {
        Module GetById(Guid id);
        Module GetById(Guid siteId, Guid id);
        int GetCountByModuleTypeId(Guid moduleTypeId);
        int GetCountByModuleId(Guid moduleId);
        void Create(Module module);
        void Update(Module module);
    }
}