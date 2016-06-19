using System;
using Weapsy.Core.Domain;

namespace Weapsy.Domain.Model.Menus.Rules
{
    public interface IMenuRules : IRules<Menu>
    {
        bool IsMenuIdUnique(Guid id);
        bool IsMenuNameValid(string name);
        bool IsMenuNameUnique(Guid siteId, string name);
    }
}
