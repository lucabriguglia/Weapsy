using System;
using Weapsy.Framework.Domain;

namespace Weapsy.Domain.Modules.Rules
{
    public interface IModuleRules : IRules<Module>
    {
        bool DoesModuleExist(Guid siteId, Guid id);
        bool IsModuleIdUnique(Guid id);
        bool IsModuleInUse(Guid id);
    }
}
