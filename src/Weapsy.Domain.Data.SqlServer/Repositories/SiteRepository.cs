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
        private readonly WeapsyDbContext _context;
        private readonly DbSet<SiteDbEntity> _sites;
        private readonly DbSet<SiteLocalisationDbEntity> _siteLocalisations;
        private readonly IMapper _mapper;

        public SiteRepository(WeapsyDbContext context, IMapper mapper)
        {
            _context = context;
            _sites = context.Set<SiteDbEntity>();
            _siteLocalisations = context.Set<SiteLocalisationDbEntity>();
            _mapper = mapper;
        }

        public Site GetById(Guid id)
        {
            var dbEntity = _sites.FirstOrDefault(x => x.Id.Equals(id));
            return dbEntity != null ? _mapper.Map<Site>(dbEntity) : null;
        }

        public Site GetByName(string name)
        {
            var dbEntity = _sites.FirstOrDefault(x => x.Name == name);
            return dbEntity != null ? _mapper.Map<Site>(dbEntity) : null;
        }

        public Site GetByUrl(string url)
        {
            var dbEntity = _sites.FirstOrDefault(x => x.Url == url);
            return dbEntity != null ? _mapper.Map<Site>(dbEntity) : null;
        }

        public ICollection<Site> GetAll()
        {
            var dbEntities = _sites.Where(x => x.Status != SiteStatus.Deleted).ToList();
            return _mapper.Map<IList<Site>>(dbEntities);
        }

        public void Create(Site site)
        {
            _sites.Add(_mapper.Map<SiteDbEntity>(site));
            _context.SaveChanges();
        }

        public void Update(Site site)
        {
            var dbEntity = _sites.FirstOrDefault(x => x.Id.Equals(site.Id));
            dbEntity = _mapper.Map(site, dbEntity);
            _context.SaveChanges();
        }

        //private void UpdateSiteLocalisations(Site site)
        //{
        //    foreach (var siteLocalisation in site.SiteLocalisations)
        //    {
        //        var siteLocalisationDbEntity = _siteLocalisations
        //            .FirstOrDefault(x =>
        //                x.SiteId == siteLocalisation.SiteId &&
        //                x.LanguageId == siteLocalisation.LanguageId);

        //        if (siteLocalisationDbEntity == null)
        //        {
        //            _siteLocalisations.Add(siteLocalisation.ToDbEntity());
        //        }
        //        else
        //        {
        //            siteLocalisationDbEntity = siteLocalisation.ToDbEntity(siteLocalisationDbEntity);
        //        }
        //    }
        //}
    }
}
