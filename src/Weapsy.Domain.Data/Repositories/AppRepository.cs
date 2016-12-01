using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Weapsy.Data;
using Weapsy.Domain.Apps;
using AppDbEntity = Weapsy.Data.Entities.App;

namespace Weapsy.Domain.Data.Repositories
{
    public class AppRepository : IAppRepository
    {
        private readonly WeapsyDbContext _context;
        private readonly DbSet<AppDbEntity> _entities;
        private readonly IMapper _mapper;

        public AppRepository(WeapsyDbContext context, IMapper mapper)
        {
            _context = context;
            _entities = context.Set<AppDbEntity>();
            _mapper = mapper;
        }

        public App GetById(Guid id)
        {
            var dbEntity = _entities
                .FirstOrDefault(x => x.Id == id && x.Status != AppStatus.Deleted);
            return dbEntity != null ? _mapper.Map<App>(dbEntity) : null;
        }

        public App GetByName(string name)
        {
            var dbEntity = _entities
                .FirstOrDefault(x => x.Name == name && x.Status != AppStatus.Deleted);
            return dbEntity != null ? _mapper.Map<App>(dbEntity) : null;
        }

        public App GetByFolder(string folder)
        {
            var dbEntity = _entities
                .FirstOrDefault(x => x.Folder == folder && x.Status != AppStatus.Deleted);
            return dbEntity != null ? _mapper.Map<App>(dbEntity) : null;
        }

        public void Create(App app)
        {
            var dbEntity = _mapper.Map<AppDbEntity>(app);
            _entities.Add(dbEntity);
            _context.SaveChanges();
        }

        public void Update(App app)
        {
            var dbEntity = _entities.FirstOrDefault(x => x.Id.Equals(app.Id));
            dbEntity = _mapper.Map(app, dbEntity);
            _context.SaveChanges();
        }
    }
}
