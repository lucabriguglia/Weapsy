using System;
using Weapsy.Core.Domain;

namespace Weapsy.Domain.Model.Modules.Rules
{
    public interface IModuleRules : IRules<Module>
    {
        bool DoesModuleExist(Guid siteId, Guid id);
        bool IsModuleIdUnique(Guid id);
        bool IsModuleInUse(Guid id);
    }
}
