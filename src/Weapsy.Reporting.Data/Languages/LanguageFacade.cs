using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Weapsy.Data;
using Weapsy.Domain.Languages;
using Weapsy.Infrastructure.Caching;
using Weapsy.Reporting.Languages;

namespace Weapsy.Reporting.Data.Languages
{
    public class LanguageFacade : ILanguageFacade
    {
        private readonly IWeapsyDbContextFactory _dbContextFactory;
        private readonly ICacheManager _cacheManager;
        private readonly IMapper _mapper;

        public LanguageFacade(IWeapsyDbContextFactory dbContextFactory,
            ICacheManager cacheManager, 
            IMapper mapper)
        {
            _dbContextFactory = dbContextFactory;
            _cacheManager = cacheManager;
            _mapper = mapper;
        }

        public IEnumerable<LanguageInfo> GetAllActive(Guid siteId)
        {
            return _cacheManager.Get(string.Format(CacheKeys.LanguagesCacheKey, siteId), () =>
            {
                using (var context = _dbContextFactory.Create())
                {
                    var dbEntities = context.Languages
                        .Where(x => x.SiteId == siteId && x.Status == LanguageStatus.Active)
                        .OrderBy(x => x.SortOrder)
                        .ToList();

                    return _mapper.Map<IEnumerable<LanguageInfo>>(dbEntities);
                }
            });
        }

        public IEnumerable<LanguageAdminModel> GetAllForAdmin(Guid siteId)
        {
            using (var context = _dbContextFactory.Create())
            {
                var dbEntities = context.Languages
                    .Where(x => x.SiteId == siteId && x.Status != LanguageStatus.Deleted)
                    .OrderBy(x => x.SortOrder)
                    .ToList();

                return _mapper.Map<IEnumerable<LanguageAdminModel>>(dbEntities);
            }
        }

        public LanguageAdminModel GetForAdmin(Guid siteId, Guid id)
        {
            using (var context = _dbContextFactory.Create())
            {
                var dbEntity = context.Languages.FirstOrDefault(x => x.SiteId == siteId && x.Id == id && x.Status != LanguageStatus.Deleted);
                return dbEntity != null ? _mapper.Map<LanguageAdminModel>(dbEntity) : null;
            }
        }
    }
}
