using System;
using System.Text.RegularExpressions;
using Weapsy.Domain.Modules;

namespace Weapsy.Domain.ModuleTypes.Rules
{
    public class ModuleTypeRules : IModuleTypeRules
    {
        private readonly IModuleTypeRepository _moduleTypeRepository;
        private readonly IModuleRepository _moduleRepository;

        public ModuleTypeRules(IModuleTypeRepository moduleTypeRepository, IModuleRepository moduleRepository)
        {
            _moduleTypeRepository = moduleTypeRepository;
            _moduleRepository = moduleRepository;
        }

        public bool DoesModuleTypeExist(Guid id)
        {
            var moduleType = _moduleTypeRepository.GetById(id);
            return moduleType != null && moduleType.Status != ModuleTypeStatus.Deleted;
        }

        public bool IsModuleTypeIdUnique(Guid id)
        {
            return _moduleTypeRepository.GetById(id) == null;
        }

        public bool IsModuleTypeNameValid(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return false;
            var regex = new Regex(@"^[A-Za-z\d\s_-]+$");
            var match = regex.Match(name);
            return match.Success;
        }

        public bool IsModuleTypeNameUnique(string name, Guid id = default(Guid))
        {
            var moduleType = _moduleTypeRepository.GetByName(name);
            return moduleType == null
                || moduleType.Status == ModuleTypeStatus.Deleted
                || (id != Guid.Empty && moduleType.Id == id);
        }

        public bool IsModuleTypeViewComponentNameUnique(string viewComponentName, Guid id = default(Guid))
        {
            var moduleType = _moduleTypeRepository.GetByViewComponentName(viewComponentName);
            return moduleType == null
                || moduleType.Status == ModuleTypeStatus.Deleted
                || (id != Guid.Empty && moduleType.Id == id);
        }

        public bool IsModuleTypeInUse(Guid id)
        {
            return _moduleRepository.GetCountByModuleTypeId(id) > 0;
        }
    }
}
