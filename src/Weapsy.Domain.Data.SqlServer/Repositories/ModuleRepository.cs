using System;
using System.Linq;
using Weapsy.Domain.Model.Modules;
using ModuleDbEntity = Weapsy.Domain.Data.Entities.Module;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace Weapsy.Domain.Data.Repositories
{
    public class ModuleRepository : IModuleRepository
    {
        private readonly WeapsyDbContext _context;
        private readonly DbSet<ModuleDbEntity> _entities;
        private readonly IMapper _mapper;

        public ModuleRepository(WeapsyDbContext context, IMapper mapper)
        {
            _context = context;
            _entities = context.Set<ModuleDbEntity>();
            _mapper = mapper;
        }

        public Module GetById(Guid id)
        {
            var dbEntity = _entities.FirstOrDefault(x => x.Id == id);
            return dbEntity != null ? _mapper.Map<Module>(dbEntity) : null;
        }

        public Module GetById(Guid siteId, Guid id)
        {
            var dbEntity = _entities.FirstOrDefault(x => x.SiteId ==  siteId && x.Id == id);
            return dbEntity != null ? _mapper.Map<Module>(dbEntity) : null;
        }

        public ICollection<Module> GetAll()
        {
            var dbEntities = _entities.Where(x => x.Status != ModuleStatus.Deleted).ToList();
            return _mapper.Map<ICollection<Module>>(dbEntities);
        }

        public int GetCountByModuleTypeId(Guid moduleTypeId)
        {
            return _entities.Where(x => x.ModuleTypeId == moduleTypeId && x.Status != ModuleStatus.Deleted).Count();
        }

        public int GetCountByModuleId(Guid moduleId)
        {
            return _entities.Where(x => x.Id == moduleId && x.Status != ModuleStatus.Deleted).Count();
        }

        public void Create(Module module)
        {
            _entities.Add(_mapper.Map<ModuleDbEntity>(module));
            _context.SaveChanges();
        }

        public void Update(Module module)
        {
            var entity = _entities.FirstOrDefault(x => x.Id == module.Id);
            entity = _mapper.Map(module, entity);
            _context.SaveChanges();
        }
    }
}
