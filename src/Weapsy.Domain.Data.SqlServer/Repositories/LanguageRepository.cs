using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Weapsy.Domain.Languages;
using LanguageDbEntity = Weapsy.Domain.Data.SqlServer.Entities.Language;

namespace Weapsy.Domain.Data.SqlServer.Repositories
{
    public class LanguageRepository : ILanguageRepository
    {
        private readonly WeapsyDbContext _context;
        private readonly DbSet<LanguageDbEntity> _entities;
        private readonly IMapper _mapper;

        public LanguageRepository(WeapsyDbContext context, IMapper mapper)
        {
            _context = context;
            _entities = context.Set<LanguageDbEntity>();
            _mapper = mapper;
        }

        public Language GetById(Guid id)
        {
            var dbEntity = _entities.FirstOrDefault(x => x.Id == id);
            return dbEntity != null ? _mapper.Map<Language>(dbEntity) : null;
        }

        public Language GetById(Guid siteId, Guid id)
        {
            var dbEntity = _entities.FirstOrDefault(x => x.SiteId == siteId && x.Id == id);
            return dbEntity != null ? _mapper.Map<Language>(dbEntity) : null;
        }

        public Language GetByName(Guid siteId, string name)
        {
            var dbEntity = _entities.FirstOrDefault(x => x.SiteId == siteId && x.Name == name);
            return dbEntity != null ? _mapper.Map<Language>(dbEntity) : null;
        }

        public Language GetByCultureName(Guid siteId, string cultureName)
        {
            var dbEntity = _entities.FirstOrDefault(x => x.SiteId == siteId && x.CultureName == cultureName);
            return dbEntity != null ? _mapper.Map<Language>(dbEntity) : null;
        }

        public Language GetByUrl(Guid siteId, string url)
        {
            var dbEntity = _entities.FirstOrDefault(x => x.SiteId == siteId && x.Url == url);
            return dbEntity != null ? _mapper.Map<Language>(dbEntity) : null;
        }

        public ICollection<Language> GetAll(Guid siteId)
        {
            var dbEntities = _entities
                .Where(x =>
                    x.SiteId == siteId &&
                    x.Status != LanguageStatus.Deleted)
                .OrderBy(x => x.SortOrder)
                .ToList();
            return _mapper.Map<ICollection<Language>>(dbEntities);
        }

        public int GetLanguagesCount(Guid siteId)
        {
            return _entities.Count(x => x.SiteId == siteId && x.Status != LanguageStatus.Deleted);
        }

        public int GetActiveLanguagesCount(Guid siteId)
        {
            return _entities.Count(x => x.SiteId == siteId && x.Status == LanguageStatus.Active);
        }

        public IEnumerable<Guid> GetLanguagesIdList(Guid siteId)
        {
            return _entities.Where(x => x.SiteId == siteId && x.Status != LanguageStatus.Deleted).Select(x => x.Id);
        }

        public void Create(Language language)
        {
            _entities.Add(_mapper.Map<LanguageDbEntity>(language));
            _context.SaveChanges();
        }

        public void Update(Language language)
        {
            var dbEntity = _entities.FirstOrDefault(x => x.Id == language.Id);
            dbEntity = _mapper.Map(language, dbEntity);
            _context.SaveChanges();
        }

        public void Update(IEnumerable<Language> languages)
        {
            foreach (var language in languages)
            {
                var dbEntity = _entities.FirstOrDefault(x => x.Id.Equals(language.Id));
                dbEntity = _mapper.Map(language, dbEntity);
            }
            _context.SaveChanges();
        }
    }
}
