using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Weapsy.Domain.Languages;
using Weapsy.Framework.Caching;
using Weapsy.Framework.Queries;
using Weapsy.Reporting.Languages;
using Weapsy.Reporting.Languages.Queries;

namespace Weapsy.Data.Reporting.Languages
{
    public class GetAllActiveHandler : IQueryHandlerAsync<GetAllActive, IEnumerable<LanguageInfo>>
    {
        private readonly IContextFactory _contextFactory;
        private readonly IMapper _mapper;
        private readonly ICacheManager _cacheManager;

        public GetAllActiveHandler(IContextFactory contextFactory, IMapper mapper, ICacheManager cacheManager)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
            _cacheManager = cacheManager;
        }

        public async Task<IEnumerable<LanguageInfo>> RetrieveAsync(GetAllActive query)
        {
            return await _cacheManager.GetAsync(string.Format(CacheKeys.LanguagesCacheKey, query.SiteId), async () =>
            {
                using (var context = _contextFactory.Create())
                {
                    var dbEntities = await context.Languages
                        .Where(x => x.SiteId == query.SiteId && x.Status == LanguageStatus.Active)
                        .OrderBy(x => x.SortOrder)
                        .ToListAsync();

                    return _mapper.Map<IEnumerable<LanguageInfo>>(dbEntities);
                }
            });
        }
    }
}
