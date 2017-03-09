using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Weapsy.Domain.Themes;
using Weapsy.Infrastructure.Caching;
using Weapsy.Infrastructure.Queries;
using Weapsy.Reporting.Themes;
using Weapsy.Reporting.Languages.Queries;

namespace Weapsy.Data.Reporting.Themes
{
    public class GetActiveThemesHandler : IQueryHandlerAsync<GetActiveThemes, IEnumerable<ThemeInfo>>
    {
        private readonly IContextFactory _contextFactory;
        private readonly IMapper _mapper;
        private readonly ICacheManager _cacheManager;

        public GetActiveThemesHandler(IContextFactory contextFactory, IMapper mapper, ICacheManager cacheManager)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
            _cacheManager = cacheManager;
        }

        public async Task<IEnumerable<ThemeInfo>> RetrieveAsync(GetActiveThemes query)
        {
            return await _cacheManager.GetAsync(string.Format(CacheKeys.ThemesCacheKey), async () =>
            {
                using (var context = _contextFactory.Create())
                {
                    var entities = await context.Themes
                        .Where(x => x.Status == ThemeStatus.Active)
                        .OrderBy(x => x.SortOrder)
                        .ToListAsync();

                    return _mapper.Map<IEnumerable<ThemeInfo>>(entities);
                }
            });
        }
    }
}
