using AutoMapper;
using System;
using System.Threading.Tasks;
using Weapsy.Core.Caching;
using Weapsy.Domain.Model.Languages;
using Weapsy.Domain.Model.Sites;
using Weapsy.Reporting.Sites;

namespace Weapsy.Reporting.Data.Sites
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

        public async Task<SiteSettings> GetSiteSettings(string name)
        {
            var site = _siteRepository.GetByName(name);
            if (site == null || site.Status == SiteStatus.Deleted)
                return null;
            return _mapper.Map<SiteSettings>(site);
        }

        public Task<SiteAdminModel> GetAdminModel(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
