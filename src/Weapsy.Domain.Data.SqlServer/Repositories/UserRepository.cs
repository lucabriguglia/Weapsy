using System;
using System.Linq;
using Weapsy.Domain.Model.Users;
using UserDbEntity = Weapsy.Domain.Data.Entities.User;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace Weapsy.Domain.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly WeapsyDbContext _context;
        private readonly DbSet<UserDbEntity> _entities;
        private readonly IMapper _mapper;

        public UserRepository(WeapsyDbContext context, IMapper mapper)
        {
            _context = context;
            _entities = context.Set<UserDbEntity>();
            _mapper = mapper;
        }

        public User GetById(Guid id)
        {
            var dbEntity = _entities.FirstOrDefault(x => x.Id.Equals(id));
            return dbEntity != null ? _mapper.Map<User>(dbEntity) : null;
        }

        public User GetByUserName(string userName)
        {
            var dbEntity = _entities.FirstOrDefault(x => x.UserName == userName);
            return dbEntity != null ? _mapper.Map<User>(dbEntity) : null;
        }

        public User GetByEmail(string email)
        {
            var dbEntity = _entities.FirstOrDefault(x => x.Email == email);
            return dbEntity != null ? _mapper.Map<User>(dbEntity) : null;
        }

        public void Create(User user)
        {
            var dbEntity = _mapper.Map<UserDbEntity>(user);
            _entities.Add(dbEntity);
            _context.SaveChanges();
        }

        public void Update(User user)
        {
            var dbEntity = _entities.FirstOrDefault(x => x.Id.Equals(user.Id));
            dbEntity = _mapper.Map(user, dbEntity);
            _context.SaveChanges();
        }
    }
}
