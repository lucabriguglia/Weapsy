using System;

namespace Weapsy.Domain.Modules.Rules
{
    public class ModuleRules : IModuleRules
    {
        private readonly IModuleRepository _moduleRepository;

        public ModuleRules(IModuleRepository moduleRepository)
        {
            _moduleRepository = moduleRepository;
        }

        public bool DoesModuleExist(Guid siteId, Guid id)
        {
            var module = _moduleRepository.GetById(siteId, id);
            return module != null && module.Status != ModuleStatus.Deleted;
        }

        public bool IsModuleIdUnique(Guid id)
        {
            return _moduleRepository.GetById(id) == null;
        }

        public bool IsModuleInUse(Guid id)
        {
            return _moduleRepository.GetCountByModuleId(id) > 0;
        }
    }
}