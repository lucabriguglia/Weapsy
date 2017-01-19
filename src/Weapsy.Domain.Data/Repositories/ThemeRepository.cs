using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Weapsy.Data;
using Weapsy.Domain.Themes;
using Theme = Weapsy.Domain.Themes.Theme;
using ThemeDbEntity = Weapsy.Data.Entities.Theme;

namespace Weapsy.Domain.Data.Repositories
{
    public class ThemeRepository : IThemeRepository
    {
        private readonly IDbContextFactory _dbContextFactory;
        private readonly IMapper _mapper;

        public ThemeRepository(IDbContextFactory dbContextFactory, IMapper mapper)
        {
            _dbContextFactory = dbContextFactory;
            _mapper = mapper;
        }

        public Theme GetById(Guid id)
        {
            using (var context = _dbContextFactory.Create())
            {
                var dbEntity = context.Themes
                    .FirstOrDefault(x => x.Id == id && x.Status != ThemeStatus.Deleted);
                return dbEntity != null ? _mapper.Map<Theme>(dbEntity) : null;
            }
        }

        public Theme GetByName(string name)
        {
            using (var context = _dbContextFactory.Create())
            {
                var dbEntity = context.Themes
                    .FirstOrDefault(x => x.Name == name && x.Status != ThemeStatus.Deleted);
                return dbEntity != null ? _mapper.Map<Theme>(dbEntity) : null;
            }
        }

        public Theme GetByFolder(string folder)
        {
            using (var context = _dbContextFactory.Create())
            {
                var dbEntity = context.Themes
                    .FirstOrDefault(x => x.Folder == folder && x.Status != ThemeStatus.Deleted);
                return dbEntity != null ? _mapper.Map<Theme>(dbEntity) : null;
            }
        }

        public int GetThemesCount()
        {
            using (var context = _dbContextFactory.Create())
            {
                return context.Themes.Count(x => x.Status != ThemeStatus.Deleted);
            }
        }

        public void Create(Theme theme)
        {
            using (var context = _dbContextFactory.Create())
            {
                var dbEntity = _mapper.Map<ThemeDbEntity>(theme);
                context.Add(dbEntity);
                context.SaveChanges();
            }
        }

        public void Update(Theme theme)
        {
            using (var context = _dbContextFactory.Create())
            {
                var dbEntity = _mapper.Map<ThemeDbEntity>(theme);
                context.Update(dbEntity);
                context.SaveChanges();
            }
        }

        public void Update(IEnumerable<Theme> themes)
        {
            using (var context = _dbContextFactory.Create())
            {
                foreach (var theme in themes)
                {
                    var dbEntity = _mapper.Map<ThemeDbEntity>(theme);
                    context.Update(dbEntity);
                }
                context.SaveChanges();
            }
        }
    }
}
