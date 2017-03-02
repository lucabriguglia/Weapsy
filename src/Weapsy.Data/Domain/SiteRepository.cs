using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Weapsy.Domain.Sites;
using SiteDbEntity = Weapsy.Data.Entities.Site;
using SiteLocalisationDbEntity = Weapsy.Data.Entities.SiteLocalisation;

namespace Weapsy.Data.Domain
{
    public class SiteRepository : ISiteRepository
    {
        private readonly IContextFactory _dbContextFactory;
        private readonly IMapper _mapper;

        public SiteRepository(IContextFactory dbContextFactory, IMapper mapper)
        {
            _dbContextFactory = dbContextFactory;
            _mapper = mapper;
        }

        public Site GetById(Guid id)
        {
            using (var context = _dbContextFactory.Create())
            {
                var dbEntity = context.Sites
                    .Include(x => x.SiteLocalisations)
                    .FirstOrDefault(x => x.Id == id && x.Status != SiteStatus.Deleted);
                return dbEntity != null ? _mapper.Map<Site>(dbEntity) : null;
            }
        }

        public Site GetByName(string name)
        {
            using (var context = _dbContextFactory.Create())
            {
                var dbEntity = context.Sites
                    .Include(x => x.SiteLocalisations)
                    .FirstOrDefault(x => x.Name == name && x.Status != SiteStatus.Deleted);
                return dbEntity != null ? _mapper.Map<Site>(dbEntity) : null;
            }
        }

        public Site GetByUrl(string url)
        {
            using (var context = _dbContextFactory.Create())
            {
                var dbEntity = context.Sites
                    .Include(x => x.SiteLocalisations)
                    .FirstOrDefault(x => x.Url == url && x.Status != SiteStatus.Deleted);
                return dbEntity != null ? _mapper.Map<Site>(dbEntity) : null;
            }
        }

        public void Create(Site site)
        {
            using (var context = _dbContextFactory.Create())
            {
                var dbEntity = _mapper.Map<SiteDbEntity>(site);
                context.Sites.Add(dbEntity);
                context.SaveChanges();
            }
        }

        public void Update(Site site)
        {
            using (var context = _dbContextFactory.Create())
            {
                var siteDbEntity = _mapper.Map<SiteDbEntity>(site);

                context.Entry(siteDbEntity).State = EntityState.Modified;

                UpdateSiteLocalisations(context, siteDbEntity.Id, siteDbEntity.SiteLocalisations);

                context.SaveChanges();
            }
        }

        private void UpdateSiteLocalisations(WeapsyDbContext context, Guid siteId, IEnumerable<SiteLocalisationDbEntity> siteLocalisations)
        {
            var currentSiteLocalisations = context.SiteLocalisations
                .AsNoTracking()
                .Where(x => x.SiteId == siteId)
                .ToList();

            foreach (var currentSiteLocalisation in currentSiteLocalisations)
            {
                var siteLocalisation = siteLocalisations
                    .FirstOrDefault(x => x.LanguageId == currentSiteLocalisation.LanguageId);

                if (siteLocalisation == null)
                    context.Remove(currentSiteLocalisation);
            }

            foreach (var siteLocalisation in siteLocalisations)
            {
                var currentSiteLocalisation = context.SiteLocalisations
                    .AsNoTracking()
                    .FirstOrDefault(x =>
                        x.SiteId == siteLocalisation.SiteId &&
                        x.LanguageId == siteLocalisation.LanguageId);

                if (currentSiteLocalisation == null)
                    context.Add(siteLocalisation);
                else
                    context.Entry(siteLocalisation).State = EntityState.Modified;
            }
        }
    }
}
