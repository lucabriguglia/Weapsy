using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Weapsy.Core.Caching;
using Weapsy.Domain.Languages;
using Weapsy.Reporting.Languages;

namespace Weapsy.Reporting.Data.Default.Languages
{
    public class LanguageFacade : ILanguageFacade
    {
        private readonly ILanguageRepository _languageRepository;
        private readonly ICacheManager _cacheManager;
        private readonly IMapper _mapper;

        public LanguageFacade(ILanguageRepository languageRepository, 
            ICacheManager cacheManager, 
            IMapper mapper)
        {
            _languageRepository = languageRepository;
            _cacheManager = cacheManager;
            _mapper = mapper;
        }

        public IEnumerable<LanguageInfo> GetAllActive(Guid siteId)
        {
            return _cacheManager.Get(string.Format(CacheKeys.LanguagesCacheKey, siteId), () =>
            {
                var languages = _languageRepository.GetAll(siteId).Where(x => x.Status != LanguageStatus.Deleted);
                return _mapper.Map<IEnumerable<LanguageInfo>>(languages);
            });
        }

        public async Task<IEnumerable<LanguageAdminModel>> GetAllForAdminAsync(Guid siteId)
        {
            var languages = _languageRepository.GetAll(siteId).Where(x => x.Status != LanguageStatus.Deleted);
            return _mapper.Map<IEnumerable<LanguageAdminModel>>(languages);
        }

        public async Task<LanguageAdminModel> GetForAdminAsync(Guid siteId, Guid id)
        {
            var language = _languageRepository.GetById(siteId, id);
            if (language == null || language.Status == LanguageStatus.Deleted)
                return null;
            return _mapper.Map<LanguageAdminModel>(language);
        }
    }
}