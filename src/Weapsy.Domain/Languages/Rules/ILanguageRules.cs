using System;
using System.Collections.Generic;
using Weapsy.Framework.Domain;

namespace Weapsy.Domain.Languages.Rules
{
    public interface ILanguageRules : IRules<Language>
    {
        bool DoesLanguageExist(Guid id);
        bool DoesLanguageExist(Guid siteId, Guid id);
        bool IsLanguageIdUnique(Guid id);
        bool IsLanguageNameValid(string name);
        bool IsLanguageNameUnique(Guid siteId, string name, Guid languageId = new Guid());
        bool IsCultureNameValid(string name);
        bool IsCultureNameUnique(Guid siteId, string name, Guid languageId = new Guid());
        bool IsLanguageUrlValid(string url);
        bool IsLanguageUrlUnique(Guid siteId, string url, Guid languageId = new Guid());
        bool AreAllSupportedLanguagesIncluded(Guid siteId, IEnumerable<Guid> languageIdList);
    }
}
