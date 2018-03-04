using System;

namespace Weapsy.Domain.Modules.Rules
{
    public interface IModuleRules
    {
        bool DoesModuleExist(Guid siteId, Guid id);
        bool IsModuleIdUnique(Guid id);
        bool IsModuleInUse(Guid id);
    }
}
