using System;
using System.Threading.Tasks;
using AutoMapper;
using Weapsy.Apps.Text.Domain;
using Weapsy.Apps.Text.Reporting;
using Weapsy.Framework.Caching;
using Weapsy.Framework.Queries;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Weapsy.Apps.Text.Data.Reporting
{
    public class GetContentHandler : IQueryHandlerAsync<GetContent, string>
    {
        private readonly IContextFactory _contextFactory;
        private readonly IMapper _mapper;
        private readonly ICacheManager _cacheManager;

        public GetContentHandler(IContextFactory contextFactory, IMapper mapper, ICacheManager cacheManager)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
            _cacheManager = cacheManager;
        }

        public async Task<string> RetrieveAsync(GetContent query)
        {
            return await _cacheManager.Get(string.Format(CacheKeys.TextModuleCacheKey, query.ModuleId, query.LanguageId), async () =>
            {
                using (var context = _contextFactory.Create())
                {
                    var module = await context.TextModules.FirstOrDefaultAsync(x => x.ModuleId == query.ModuleId && x.Status == TextModuleStatus.Active);

                    if (module == null)
                        return null;

                    module.TextVersions = await context.TextVersions
                        .Include(y => y.TextLocalisations)
                        .Where(x => x.TextModuleId == module.Id && x.Status != TextVersionStatus.Deleted)
                        .ToListAsync();

                    var textModule = _mapper.Map<TextModule>(module);

                    var publishedVersion = textModule.TextVersions.FirstOrDefault(x => x.Status == TextVersionStatus.Published);

                    var content = publishedVersion != null ? publishedVersion.Content : string.Empty;

                    if (query.LanguageId != Guid.Empty && publishedVersion != null)
                    {
                        var localisedVersion =
                            publishedVersion.TextLocalisations.FirstOrDefault(x => x.LanguageId == query.LanguageId);

                        if (!string.IsNullOrEmpty(localisedVersion?.Content))
                            content = localisedVersion.Content;
                    }

                    return content;
                }
            });
        }
    }
}
