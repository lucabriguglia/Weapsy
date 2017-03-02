using System;
using System.Linq;
using AutoMapper;
using Weapsy.Domain.Modules;
using ModuleDbEntity = Weapsy.Data.Entities.Module;

namespace Weapsy.Data.Domain
{
    public class ModuleRepository : IModuleRepository
    {
        private readonly IContextFactory _dbContextFactory;
        private readonly IMapper _mapper;

        public ModuleRepository(IContextFactory dbContextFactory, IMapper mapper)
        {
            _dbContextFactory = dbContextFactory;
            _mapper = mapper;
        }

        public Module GetById(Guid id)
        {
            using (var context = _dbContextFactory.Create())
            {
                var dbEntity = context.Modules
                    .FirstOrDefault(x => x.Id == id && x.Status != ModuleStatus.Deleted);
                return dbEntity != null ? _mapper.Map<Module>(dbEntity) : null;
            }
        }

        public Module GetById(Guid siteId, Guid id)
        {
            using (var context = _dbContextFactory.Create())
            {
                var dbEntity = context.Modules
                    .FirstOrDefault(x => x.SiteId ==  siteId && x.Id == id && x.Status != ModuleStatus.Deleted);
                return dbEntity != null ? _mapper.Map<Module>(dbEntity) : null;
            }
        }

        public int GetCountByModuleTypeId(Guid moduleTypeId)
        {
            using (var context = _dbContextFactory.Create())
            {
                return context.Modules
                    .Count(x => x.ModuleTypeId == moduleTypeId && x.Status != ModuleStatus.Deleted);
            }
        }

        public int GetCountByModuleId(Guid moduleId)
        {
            using (var context = _dbContextFactory.Create())
            {
                return context.Modules
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
