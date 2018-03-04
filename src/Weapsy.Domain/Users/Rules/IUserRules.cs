using System;
using Weapsy.Framework.Domain;

namespace Weapsy.Domain.Users.Rules
{
    public interface IUserRules
    {
        bool IsUserNameUnique(string name, Guid userId = new Guid());
        bool IsUserEmailUnique(string email, Guid userId = new Guid());
    }
}
