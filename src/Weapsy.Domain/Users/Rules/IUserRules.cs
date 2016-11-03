using System;
using Weapsy.Infrastructure.Domain;

namespace Weapsy.Domain.Users.Rules
{
    public interface IUserRules : IRules<User>
    {
        bool DoesUserExist(Guid id);
        bool IsUserIdUnique(Guid id);
        bool IsUserNameUnique(string name, Guid userId = new Guid());
        bool IsUserEmailUnique(string email, Guid userId = new Guid());
    }
}
