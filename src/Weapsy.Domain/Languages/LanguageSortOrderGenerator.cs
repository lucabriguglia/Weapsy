using System;

namespace Weapsy.Domain.Languages
{
    public class LanguageSortOrderGenerator : ILanguageSortOrderGenerator
    {
        private readonly ILanguageRepository _languageRepository;

        public LanguageSortOrderGenerator(ILanguageRepository languageRepository)
        {
            _languageRepository = languageRepository;
        }

        public int GenerateNextSortOrder(Guid siteId)
        {
            var languagesCount = _languageRepository.GetLanguagesCount(siteId);
            return languagesCount + 1;
        }
    }
}
