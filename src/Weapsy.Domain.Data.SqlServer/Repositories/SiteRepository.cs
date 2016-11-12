using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Weapsy.Domain.Sites;
using SiteDbEntity = Weapsy.Domain.Data.SqlServer.Entities.Site;
using SiteLocalisationDbEntity = Weapsy.Domain.Data.SqlServer.Entities.SiteLocalisation;

namespace Weapsy.Domain.Data.SqlServer.Repositories
{
    public class SiteRepository : ISiteRepository
    {
        private readonly IWeapsyDbContextFactory _dbContextFactory;
        private readonly IMapper _mapper;

        public SiteRepository(IWeapsyDbContextFactory dbContextFactory, IMapper mapper)
        {
            _dbContextFactory = dbContextFactory;
            _mapper = mapper;
        }

        public Site GetById(Guid id)
        {
            using (var context = _dbContextFactory.Create())
            {
                var dbEntity = context.Set<SiteDbEntity>()
                    .Include(x => x.SiteLocalisations)
                    .FirstOrDefault(x => x.Id == id && x.Status != SiteStatus.Deleted);
                return dbEntity != null ? _mapper.Map<Site>(dbEntity) : null;
            }
        }

        public Site GetByName(string name)
        {
            using (var context = _dbContextFactory.Create())
            {
                var dbEntity = context.Set<SiteDbEntity>()
                    .Include(x => x.SiteLocalisations)
                    .FirstOrDefault(x => x.Name == name && x.Status != SiteStatus.Deleted);
                return dbEntity != null ? _mapper.Map<Site>(dbEntity) : null;
            }
        }

        public Site GetByUrl(string url)
        {
            using (var context = _dbContextFactory.Create())
            {
                var dbEntity = context.Set<SiteDbEntity>()
                    .Include(x => x.SiteLocalisations)
                    .FirstOrDefault(x => x.Url == url && x.Status != SiteStatus.Deleted);
                return dbEntity != null ? _mapper.Map<Site>(dbEntity) : null;
            }
        }

        public ICollection<Site> GetAll()
        {
            using (var context = _dbContextFactory.Create())
            {
                var dbEntities = context.Set<SiteDbEntity>()
                    .Include(x => x.SiteLocalisations)
                    .Where(x => x.Status != SiteStatus.Deleted).ToList();
                return _mapper.Map<IList<Site>>(dbEntities);
            }
        }

        public void Create(Site site)
        {
            using (var context = _dbContextFactory.Create())
            {
                var dbEntity = _mapper.Map<SiteDbEntity>(site);
                context.Set<SiteDbEntity>().Add(dbEntity);
                context.SaveChanges();
            }
        }

        public void Update(Site site)
        {
            using (var context = _dbContextFactory.Create())
            {
                var siteDbEntity = _mapper.Map<SiteDbEntity>(site);

                context.Entry(siteDbEntity).State = EntityState.Modified;

                UpdateSiteLocalisations(context, siteDbEntity.SiteLocalisations);

                context.SaveChanges();
            }
        }

        private void UpdateSiteLocalisations(WeapsyDbContext context, IEnumerable<SiteLocalisationDbEntity> siteLocalisations)
        {
            foreach (var siteLocalisation in siteLocalisations)
            {
                var currentSiteLocalisation = context.Set<SiteLocalisationDbEntity>()
                    .AsNoTracking()
                    .FirstOrDefault(x =>
                        x.SiteId == siteLocalisation.SiteId &&
                        x.LanguageId == siteLocalisation.LanguageId);

                if (currentSiteLocalisation == null)
                {
                    context.Add(siteLocalisation);
                }
                else
                {
                    context.Entry(siteLocalisation).State = EntityState.Modified;
                }
            }
        }
    }
}
