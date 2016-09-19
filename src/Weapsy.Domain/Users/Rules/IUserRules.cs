using System;
using Weapsy.Core.Domain;

namespace Weapsy.Domain.Model.Users.Rules
{
    public interface IUserRules : IRules<User>
    {
        bool DoesUserExist(Guid id);
        bool IsUserIdUnique(Guid id);
        bool IsUserNameUnique(string name, Guid userId = new Guid());
        bool IsUserEmailUnique(string email, Guid userId = new Guid());
    }
}
