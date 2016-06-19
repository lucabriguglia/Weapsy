using System;
using System.Linq;
using Weapsy.Domain.Model.ModuleTypes;
using ModuleTypeDbEntity = Weapsy.Domain.Data.Entities.ModuleType;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace Weapsy.Domain.Data.Repositories
{
    public class ModuleTypeRepository : IModuleTypeRepository
    {
        private readonly WeapsyDbContext _context;
        private readonly DbSet<ModuleTypeDbEntity> _entities;
        private readonly IMapper _mapper;

        public ModuleTypeRepository(WeapsyDbContext context, IMapper mapper)
        {
            _context = context;
            _entities = context.Set<ModuleTypeDbEntity>();
            _mapper = mapper;
        }

        public ModuleType GetById(Guid id)
        {
            var dbEntity = _entities.FirstOrDefault(x => x.Id.Equals(id));
            return dbEntity != null ? _mapper.Map<ModuleType>(dbEntity) : null;
        }

        public ModuleType GetByName(string name)
        {
            var dbEntity = _entities.FirstOrDefault(x => x.Name == name);
            return dbEntity != null ? _mapper.Map<ModuleType>(dbEntity) : null;
        }

        public ModuleType GetByViewComponentName(string viewComponentName)
        {
            var dbEntity = _entities.FirstOrDefault(x => x.ViewName == viewComponentName && x.ViewType == ViewType.ViewComponent);
            return dbEntity != null ? _mapper.Map<ModuleType>(dbEntity) : null;
        }

        public ICollection<ModuleType> GetAll()
        {
            var dbEntities = _entities.Where(x => x.Status != ModuleTypeStatus.Deleted).ToList();
            return _mapper.Map<ICollection<ModuleType>>(dbEntities);
        }

        public void Create(ModuleType moduleType)
        {
            _entities.Add(_mapper.Map<ModuleTypeDbEntity>(moduleType));
            _context.SaveChanges();
        }

        public void Update(ModuleType moduleType)
        {
            var entity = _entities.FirstOrDefault(x => x.Id == moduleType.Id);
            entity = _mapper.Map(moduleType, entity);
            _context.SaveChanges();
        }
    }
}
