using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Weapsy.Domain.Sites;
using Weapsy.Framework.Caching;
using Weapsy.Framework.Queries;
using Weapsy.Reporting.Sites;
using Weapsy.Reporting.Sites.Queries;

namespace Weapsy.Data.Reporting.Sites
{
    public class GetSiteInfoHandler : IQueryHandlerAsync<GetSiteInfo, SiteInfo>
    {
        private readonly IContextFactory _contextFactory;
        private readonly IMapper _mapper;
        private readonly ICacheManager _cacheManager;
        private readonly IQueryDispatcher _queryDispatcher;

        public GetSiteInfoHandler(IContextFactory contextFactory, 
            IMapper mapper, 
            ICacheManager cacheManager, 
            IQueryDispatcher queryDispatcher)
        {
            _contextFactory = contextFactory;
            _mapper = mapper;
            _cacheManager = cacheManager;
            _queryDispatcher = queryDispatcher;
        }

        public async Task<SiteInfo> RetrieveAsync(GetSiteInfo query)
        {
            return await _cacheManager.Get(string.Format(CacheKeys.SiteInfoCacheKey, query.Name, query.LanguageId), async () =>
            {
                using (var context = _contextFactory.Create())
                {
                    var site = await context.Sites
                        .Include(x => x.SiteLocalisations)
                        .FirstOrDefaultAsync(x => x.Name == query.Name && x.Status == SiteStatus.Active);

                    if (site == null)
                        return null;

                    var siteInfo = _mapper.Map<SiteInfo>(site);

                    var title = site.Title;
                    var metaDescription = site.MetaDescription;
                    var metaKeywords = site.MetaKeywords;

                    if (query.LanguageId != Guid.Empty)
                    {
                        var siteLocalisation = site.SiteLocalisations.FirstOrDefault(x => x.LanguageId == query.LanguageId);

                        if (siteLocalisation != null)
                        {
                            title = !string.IsNullOrWhiteSpace(siteLocalisation.Title) ? siteLocalisation.Title : title;
                            metaDescription = !string.IsNullOrWhiteSpace(siteLocalisation.MetaDescription) ? siteLocalisation.MetaDescription : metaDescription;
                            metaKeywords = !string.IsNullOrWhiteSpace(siteLocalisation.MetaKeywords) ? siteLocalisation.MetaKeywords : metaKeywords;
                        }
                    }

                    siteInfo.Title = title;
                    siteInfo.MetaDescription = metaDescription;
                    siteInfo.MetaKeywords = metaKeywords;

                    return siteInfo;
                }
            });
        }
    }
}
