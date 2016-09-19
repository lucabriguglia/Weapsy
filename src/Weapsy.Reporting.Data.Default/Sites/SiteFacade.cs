using System;
using System.Threading.Tasks;
using AutoMapper;
using Weapsy.Core.Caching;
using Weapsy.Domain.Languages;
using Weapsy.Domain.Sites;
using Weapsy.Reporting.Sites;

namespace Weapsy.Reporting.Data.Default.Sites
{
    public class SiteFacade : ISiteFacade
    {
        private readonly ISiteRepository _siteRepository;
        private readonly ILanguageRepository _languageRepository;
        private readonly ICacheManager _cacheManager;
        private readonly IMapper _mapper;

        public SiteFacade(ISiteRepository siteRepository, 
            ILanguageRepository languageRepository, 
            ICacheManager cacheManager,
            IMapper mapper)
        {
            _siteRepository = siteRepository;
            _languageRepository = languageRepository;
            _cacheManager = cacheManager;
            _mapper = mapper;
        }

        public async Task<SiteInfo> GetSiteInfo(string name)
        {
            var site = _siteRepository.GetByName(name);

            if (site == null || site.Status == SiteStatus.Deleted)
                return null;

            return _mapper.Map<SiteInfo>(site);
        }

        public async Task<SiteAdminModel> GetAdminModel(Guid id)
        {
            var site = _siteRepository.GetById(id);

            if (site == null || site.Status == SiteStatus.Deleted)
                return null;

            return _mapper.Map<SiteAdminModel>(site);
        }
    }
}
