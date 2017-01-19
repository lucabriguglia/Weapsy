using System;
using System.Linq;
using AutoMapper;
using Weapsy.Data;
using Weapsy.Domain.Users;
using UserDbEntity = Weapsy.Data.Entities.User;

namespace Weapsy.Domain.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbContextFactory _dbContextFactory;
        private readonly IMapper _mapper;

        public UserRepository(IDbContextFactory dbContextFactory, IMapper mapper)
        {
            _dbContextFactory = dbContextFactory;
            _mapper = mapper;
        }

        public User GetById(Guid id)
        {
            using (var context = _dbContextFactory.Create())
            {
                var dbEntity = context.Users.FirstOrDefault(x => x.Id == id && x.Status != UserStatus.Deleted);
                return dbEntity != null ? _mapper.Map<User>(dbEntity) : null;
            }
        }

        public User GetByUserName(string userName)
        {
            using (var context = _dbContextFactory.Create())
            {
                var dbEntity = context.Users.FirstOrDefault(x => x.UserName == userName && x.Status != UserStatus.Deleted);
                return dbEntity != null ? _mapper.Map<User>(dbEntity) : null;
            }
        }

        public User GetByEmail(string email)
        {
            using (var context = _dbContextFactory.Create())
            {
                var dbEntity = context.Users.FirstOrDefault(x => x.Email == email && x.Status != UserStatus.Deleted);
                return dbEntity != null ? _mapper.Map<User>(dbEntity) : null;
            }
        }

        public void Create(User user)
        {
            using (var context = _dbContextFactory.Create())
            {
                var dbEntity = _mapper.Map<UserDbEntity>(user);
                context.Users.Add(dbEntity);
                context.SaveChanges();
            }
        }

        public void Update(User user)
        {
            using (var context = _dbContextFactory.Create())
            {
                var dbEntity = context.Users.FirstOrDefault(x => x.Id.Equals(user.Id));
                _mapper.Map(user, dbEntity);
                context.SaveChanges();
            }
        }
    }
}
