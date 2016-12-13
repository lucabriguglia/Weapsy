using System;
using Weapsy.Infrastructure.Domain;

namespace Weapsy.Domain.ModuleTypes
{
    public interface IModuleTypeRepository : IRepository<ModuleType>
    {
        ModuleType GetById(Guid id);
        ModuleType GetByName(string name);
        ModuleType GetByViewComponentName(string viewComponentName);
        void Create(ModuleType moduleType);
        void Update(ModuleType moduleType);
    }
}
