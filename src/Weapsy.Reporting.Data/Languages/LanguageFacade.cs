using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Weapsy.Data;
using Weapsy.Domain.Languages;
using Weapsy.Infrastructure.Caching;
using Weapsy.Reporting.Languages;

namespace Weapsy.Reporting.Data.Languages
{
    public class LanguageFacade : ILanguageFacade
    {
        private readonly IDbContextFactory _dbContextFactory;
        private readonly ICacheManager _cacheManager;
        private readonly IMapper _mapper;

        public LanguageFacade(IDbContextFactory dbContextFactory,
            ICacheManager cacheManager, 
            IMapper mapper)
        {
            _dbContextFactory = dbContextFactory;
            _cacheManager = cacheManager;
            _mapper = mapper;
        }

        public async Task<IEnumerable<LanguageInfo>> GetAllActiveAsync(Guid siteId)
        {
            return await _cacheManager.GetAsync(string.Format(CacheKeys.LanguagesCacheKey, siteId), async () =>
            {
                using (var context = _dbContextFactory.Create())
                {
                    var dbEntities = await context.Languages
                        .Where(x => x.SiteId == siteId && x.Status == LanguageStatus.Active)
                        .OrderBy(x => x.SortOrder)
                        .ToListAsync();

                    return _mapper.Map<IEnumerable<LanguageInfo>>(dbEntities);
                }
            });
        }

        public async Task<IEnumerable<LanguageAdminModel>> GetAllForAdminAsync(Guid siteId)
        {
            using (var context = _dbContextFactory.Create())
            {
                var dbEntities = await context.Languages
                    .Where(x => x.SiteId == siteId && x.Status != LanguageStatus.Deleted)
                    .OrderBy(x => x.SortOrder)
                    .ToListAsync();

                return _mapper.Map<IEnumerable<LanguageAdminModel>>(dbEntities);
            }
        }

        public async Task<LanguageAdminModel> GetForAdminAsync(Guid siteId, Guid id)
        {
            using (var context = _dbContextFactory.Create())
            {
                var dbEntity = await context.Languages.FirstOrDefaultAsync(x => x.SiteId == siteId && x.Id == id && x.Status != LanguageStatus.Deleted);
                return dbEntity != null ? _mapper.Map<LanguageAdminModel>(dbEntity) : null;
            }
        }
    }
}
