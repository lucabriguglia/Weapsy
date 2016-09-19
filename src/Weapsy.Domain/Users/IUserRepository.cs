using System;
using Weapsy.Core.Domain;

namespace Weapsy.Domain.Users
{
    public interface IUserRepository : IRepository<User>
    {
        User GetById(Guid id);
        User GetByEmail(string email);
        User GetByUserName(string userName);
        void Create(User user);
        void Update(User user);
    }
}
