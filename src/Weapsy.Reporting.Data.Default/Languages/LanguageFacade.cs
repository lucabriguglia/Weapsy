using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Weapsy.Core.Caching;
using Weapsy.Domain.Model.Languages;
using Weapsy.Reporting.Languages;

namespace Weapsy.Reporting.Data.Languages
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

        public IEnumerable<LanguageViewModel> GetAllActive(Guid siteId)
        {
            return _cacheManager.Get(string.Format(CacheKeys.LANGUAGES_CACHE_KEY, siteId), () =>
            {
                var languages = _languageRepository.GetAll(siteId).Where(x => x.Status != LanguageStatus.Deleted);
                return _mapper.Map<IEnumerable<LanguageViewModel>>(languages);
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