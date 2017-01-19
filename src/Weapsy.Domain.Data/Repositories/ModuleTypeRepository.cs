using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Weapsy.Data;
using Weapsy.Domain.ModuleTypes;
using ModuleTypeDbEntity = Weapsy.Data.Entities.ModuleType;

namespace Weapsy.Domain.Data.Repositories
{
    public class ModuleTypeRepository : IModuleTypeRepository
    {
        private readonly IDbContextFactory _dbContextFactory;
        private readonly IMapper _mapper;

        public ModuleTypeRepository(IDbContextFactory dbContextFactory, IMapper mapper)
        {
            _dbContextFactory = dbContextFactory;
            _mapper = mapper;
            
        }

        public ModuleType GetById(Guid id)
        {
            using (var context = _dbContextFactory.Create())
            {
                var dbEntity = context.ModuleTypes
                    .FirstOrDefault(x => x.Id == id && x.Status != ModuleTypeStatus.Deleted);
                return dbEntity != null ? _mapper.Map<ModuleType>(dbEntity) : null;
            }
        }

        public ModuleType GetByName(string name)
        {
            using (var context = _dbContextFactory.Create())
            {
                var dbEntity = context.ModuleTypes
                    .FirstOrDefault(x => x.Name == name && x.Status != ModuleTypeStatus.Deleted);
                return dbEntity != null ? _mapper.Map<ModuleType>(dbEntity) : null;
            }
        }

        public ModuleType GetByViewComponentName(string viewComponentName)
        {
            using (var context = _dbContextFactory.Create())
            {
                var dbEntity = context.ModuleTypes
                    .FirstOrDefault(x => x.ViewName == viewComponentName 
                        && x.ViewType == ViewType.ViewComponent
                        && x.Status != ModuleTypeStatus.Deleted);
                return dbEntity != null ? _mapper.Map<ModuleType>(dbEntity) : null;
            }
        }

        public void Create(ModuleType moduleType)
        {
            using (var context = _dbContextFactory.Create())
            {
                var dbEntity = _mapper.Map<ModuleTypeDbEntity>(moduleType);
                context.Add(dbEntity);
                context.SaveChanges();
            }
        }

        public void Update(ModuleType moduleType)
        {
            using (var context = _dbContextFactory.Create())
            {
                var dbEntity = _mapper.Map<ModuleTypeDbEntity>(moduleType);
                context.Update(dbEntity);
                context.SaveChanges();
            }
        }
    }
}
