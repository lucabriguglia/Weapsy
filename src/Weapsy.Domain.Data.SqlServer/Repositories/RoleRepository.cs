using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Weapsy.Domain.Model.Roles;
using RoleDbEntity = Weapsy.Domain.Data.SqlServer.Entities.Role;

namespace Weapsy.Domain.Data.SqlServer.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly WeapsyDbContext _context;
        private readonly DbSet<RoleDbEntity> _entities;
        private readonly IMapper _mapper;

        public RoleRepository(WeapsyDbContext context, IMapper mapper)
        {
            _context = context;
            _entities = context.Set<RoleDbEntity>();
            _mapper = mapper;
        }

        public Role GetById(Guid id)
        {
            var dbEntity = _entities.FirstOrDefault(x => x.Id == id);
            return dbEntity != null ? _mapper.Map<Role>(dbEntity) : null;
        }

        public Role GetByName(string name)
        {
            var dbEntity = _entities.FirstOrDefault(x => x.Name == name);
            return dbEntity != null ? _mapper.Map<Role>(dbEntity) : null;
        }

        public IEnumerable<Role> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Create(Role role)
        {
            var dbEntity = _mapper.Map<RoleDbEntity>(role);
            _entities.Add(dbEntity);
            _context.SaveChanges();
        }

        public void Update(Role role)
        {
            var dbEntity = _entities.FirstOrDefault(x => x.Id == role.Id);
            _mapper.Map(role, dbEntity);
            _context.SaveChanges();
        }

        public void Delete(Role role)
        {
            var dbEntity = _entities.FirstOrDefault(x => x.Id == role.Id);
            _context.Remove(dbEntity);
            _context.SaveChanges();
        }
    }
}
