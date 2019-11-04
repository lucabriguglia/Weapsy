using System;

namespace Weapsy.Domain.Menus.Rules
{
    public interface IMenuRules
    {
        bool IsMenuIdUnique(Guid id);
        bool IsMenuNameValid(string name);
        bool IsMenuNameUnique(Guid siteId, string name);
    }
}
