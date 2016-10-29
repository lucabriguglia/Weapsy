using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Weapsy.Domain.Sites;
using SiteDbEntity = Weapsy.Domain.Data.SqlServer.Entities.Site;

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
                var dbEntity = context.Set<SiteDbEntity>().FirstOrDefault(x => x.Id == id);
                return dbEntity != null ? _mapper.Map<Site>(dbEntity) : null;
            }
        }

        public Site GetByName(string name)
        {
            using (var context = _dbContextFactory.Create())
            {
                var dbEntity = context.Set<SiteDbEntity>().FirstOrDefault(x => x.Name == name);
                return dbEntity != null ? _mapper.Map<Site>(dbEntity) : null;
            }
        }

        public Site GetByUrl(string url)
        {
            using (var context = _dbContextFactory.Create())
            {
                var dbEntity = context.Set<SiteDbEntity>().FirstOrDefault(x => x.Url == url);
                return dbEntity != null ? _mapper.Map<Site>(dbEntity) : null;
            }
        }

        public ICollection<Site> GetAll()
        {
            using (var context = _dbContextFactory.Create())
            {
                var dbEntities = context.Set<SiteDbEntity>().Where(x => x.Status != SiteStatus.Deleted).ToList();
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
                var dbEntity = context.Set<SiteDbEntity>().FirstOrDefault(x => x.Id == site.Id);
                _mapper.Map(site, dbEntity);
                context.SaveChanges();
            }
        }
    }
}
