using System;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Weapsy.Data;
using Weapsy.Domain.Users;
using UserDbEntity = Weapsy.Data.Entities.User;

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
            var dbEntity = _entities.FirstOrDefault(x => x.Id == id && x.Status != UserStatus.Deleted);
            return dbEntity != null ? _mapper.Map<User>(dbEntity) : null;
        }

        public User GetByUserName(string userName)
        {
            var dbEntity = _entities.FirstOrDefault(x => x.UserName == userName && x.Status != UserStatus.Deleted);
            return dbEntity != null ? _mapper.Map<User>(dbEntity) : null;
        }

        public User GetByEmail(string email)
        {
            var dbEntity = _entities.FirstOrDefault(x => x.Email == email && x.Status != UserStatus.Deleted);
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
            _mapper.Map(user, dbEntity);
            _context.SaveChanges();
        }
    }
}
