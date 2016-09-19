using System;
using System.Collections.Generic;
using Weapsy.Core.Domain;

namespace Weapsy.Domain.Model.ModuleTypes
{
    public interface IModuleTypeRepository : IRepository<ModuleType>
    {
        ModuleType GetById(Guid id);
        ModuleType GetByName(string name);
        ModuleType GetByViewComponentName(string viewComponentName);
        ICollection<ModuleType> GetAll();
        void Create(ModuleType moduleType);
        void Update(ModuleType moduleType);
    }
}
