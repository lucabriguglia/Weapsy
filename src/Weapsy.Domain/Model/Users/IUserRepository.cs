using System;
using Weapsy.Core.Domain;

namespace Weapsy.Domain.Model.Users
{
    public interface IUserRepository : IRepository<User>
    {
        User GetById(Guid id);
        void Create(User user);
        void Update(User user);
    }
}
