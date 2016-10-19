using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Weapsy.Domain.Themes;
using ThemeDbEntity = Weapsy.Domain.Data.SqlServer.Entities.Theme;

namespace Weapsy.Domain.Data.SqlServer.Repositories
{
    public class ThemeRepository : IThemeRepository
    {
        private readonly WeapsyDbContext _context;
        private readonly DbSet<ThemeDbEntity> _entities;
        private readonly IMapper _mapper;

        public ThemeRepository(WeapsyDbContext context, IMapper mapper)
        {
            _context = context;
            _entities = context.Set<ThemeDbEntity>();
            _mapper = mapper;
        }

        public Theme GetById(Guid id)
        {
            var dbEntity = _entities.FirstOrDefault(x => x.Id.Equals(id));
            return dbEntity != null ? _mapper.Map<Theme>(dbEntity) : null;
        }

        public Theme GetByName(string name)
        {
            var dbEntity = _entities.FirstOrDefault(x => x.Name == name);
            return dbEntity != null ? _mapper.Map<Theme>(dbEntity) : null;
        }

        public Theme GetByFolder(string folder)
        {
            var dbEntity = _entities.FirstOrDefault(x => x.Folder == folder);
            return dbEntity != null ? _mapper.Map<Theme>(dbEntity) : null;
        }

        public ICollection<Theme> GetAll()
        {
            var dbEntities = _entities
                .Where(x => x.Status != ThemeStatus.Deleted)
                .OrderBy(x => x.SortOrder)
                .ToList();
            return _mapper.Map<ICollection<Theme>>(dbEntities);
        }

        public int GetThemesCount()
        {
            return _entities.Count(x => x.Status != ThemeStatus.Deleted);
        }

        public void Create(Theme theme)
        {
            _entities.Add(_mapper.Map<ThemeDbEntity>(theme));
            _context.SaveChanges();
        }

        public void Update(Theme theme)
        {
            var entity = _entities.FirstOrDefault(x => x.Id.Equals(theme.Id));
            entity = _mapper.Map(theme, entity);
            _context.SaveChanges();
        }

        public void Update(IEnumerable<Theme> themes)
        {
            foreach (var theme in themes)
            {
                var entity = _entities.FirstOrDefault(x => x.Id.Equals(theme.Id));
                entity = _mapper.Map(theme, entity);
            }
            _context.SaveChanges();
        }
    }
}
