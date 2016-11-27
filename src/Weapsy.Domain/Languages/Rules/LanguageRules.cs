using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Weapsy.Domain.Languages.Rules
{
    public class LanguageRules : ILanguageRules
    {
        private readonly ILanguageRepository _languageRepository;

        public LanguageRules(ILanguageRepository languageRepository)
        {
            _languageRepository = languageRepository;
        }

        public bool DoesLanguageExist(Guid id)
        {
            var language = _languageRepository.GetById(id);
            return language != null && language.Status != LanguageStatus.Deleted;
        }

        public bool DoesLanguageExist(Guid siteId, Guid id)
        {
            var language = _languageRepository.GetById(siteId, id);
            return language != null && language.Status != LanguageStatus.Deleted;
        }

        public bool IsLanguageIdUnique(Guid id)
        {
            return _languageRepository.GetById(id) == null;
        }

        public bool IsLanguageNameValid(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return false;
            var regex = new Regex(@"^[A-Za-z\d\s_-]+$");
            var match = regex.Match(name);
            return match.Success;
        }

        public bool IsLanguageNameUnique(Guid siteId, string name, Guid languageId = new Guid())
        {
            var language = _languageRepository.GetByName(siteId, name);
            return IsLanguageUnique(language, languageId);
        }

        public bool IsCultureNameValid(string cultureName)
        {
            if (string.IsNullOrWhiteSpace(cultureName))
                return false;

            try
            {
                var cultureInfo = new CultureInfo(cultureName);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public bool IsCultureNameUnique(Guid siteId, string cultureName, Guid languageId = new Guid())
        {
            var language = _languageRepository.GetByCultureName(siteId, cultureName);
            return IsLanguageUnique(language, languageId);
        }

        public bool IsLanguageUrlValid(string url)
        {
            if (string.IsNullOrWhiteSpace(url)) return false;
            var regex = new Regex(@"^[A-Za-z\d_-]+$");
            var match = regex.Match(url);
            return match.Success;
        }

        public bool IsLanguageUrlUnique(Guid siteId, string url, Guid languageId = new Guid())
        {
            var language = _languageRepository.GetByUrl(siteId, url);
            return IsLanguageUnique(language, languageId);
        }

        private bool IsLanguageUnique(Language language, Guid languageId)
        {
            return language == null
                || language.Status == LanguageStatus.Deleted
                || (languageId != Guid.Empty && language.Id == languageId);
        }

        public bool AreAllSupportedLanguagesIncluded(Guid siteId, IEnumerable<Guid> languageIdList)
        {
            var supportedLanguageIdList = _languageRepository.GetLanguagesIdList(siteId);
            return new HashSet<Guid>(supportedLanguageIdList).SetEquals(languageIdList);
        }
    }
}
