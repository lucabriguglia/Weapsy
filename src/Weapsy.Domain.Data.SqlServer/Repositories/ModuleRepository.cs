using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Weapsy.Domain.Modules;
using ModuleDbEntity = Weapsy.Domain.Data.SqlServer.Entities.Module;

namespace Weapsy.Domain.Data.SqlServer.Repositories
{
    public class ModuleRepository : IModuleRepository
    {
        private readonly IWeapsyDbContextFactory _dbContextFactory;
        private readonly IMapper _mapper;

        public ModuleRepository(IWeapsyDbContextFactory dbContextFactory, IMapper mapper)
        {
            _dbContextFactory = dbContextFactory;
            _mapper = mapper;
        }

        public Module GetById(Guid id)
        {
            using (var context = _dbContextFactory.Create())
            {
                var dbEntity = context.Set<ModuleDbEntity>()
                    .FirstOrDefault(x => x.Id == id && x.Status != ModuleStatus.Deleted);
                return dbEntity != null ? _mapper.Map<Module>(dbEntity) : null;
            }
        }

        public Module GetById(Guid siteId, Guid id)
        {
            using (var context = _dbContextFactory.Create())
            {
                var dbEntity = context.Set<ModuleDbEntity>()
                    .FirstOrDefault(x => x.SiteId ==  siteId && x.Id == id && x.Status != ModuleStatus.Deleted);
                return dbEntity != null ? _mapper.Map<Module>(dbEntity) : null;
            }
        }

        public ICollection<Module> GetAll()
        {
            using (var context = _dbContextFactory.Create())
            {
                var dbEntities = context.Set<ModuleDbEntity>()
                    .Where(x => x.Status != ModuleStatus.Deleted)
                    .ToList();
                return _mapper.Map<ICollection<Module>>(dbEntities);
            }
        }

        public int GetCountByModuleTypeId(Guid moduleTypeId)
        {
            using (var context = _dbContextFactory.Create())
            {
                return context.Set<ModuleDbEntity>()
                    .Count(x => x.ModuleTypeId == moduleTypeId && x.Status != ModuleStatus.Deleted);
            }
        }

        public int GetCountByModuleId(Guid moduleId)
        {
            using (var context = _dbContextFactory.Create())
            {
                return context.Set<ModuleDbEntity>()
                    .Count(x => x.Id == moduleId && x.Status != ModuleStatus.Deleted);
            }
        }

        public void Create(Module module)
        {
            using (var context = _dbContextFactory.Create())
            {
                var dbEntity = _mapper.Map<ModuleDbEntity>(module);
                context.Add(dbEntity);
                context.SaveChanges();
            }
        }

        public void Update(Module module)
        {
            using (var context = _dbContextFactory.Create())
            {
                var dbEntity = _mapper.Map<ModuleDbEntity>(module);
                context.Update(dbEntity);
                context.SaveChanges();
            }
        }
    }
}
