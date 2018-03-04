using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Weapsy.Domain.Languages
{
    public interface ILanguageRepository
    {
        Language GetById(Guid id);
        Language GetById(Guid siteId, Guid id);
        Language GetByName(Guid siteId, string name);
        Language GetByCultureName(Guid siteId, string cultureName);
        Language GetByUrl(Guid siteId, string url);
        ICollection<Language> GetAll(Guid siteId);
        int GetLanguagesCount(Guid siteId);
        IEnumerable<Guid> GetLanguagesIdList(Guid siteId);        
        void Create(Language language);
        Task CreateAsync(Language language);
        void Update(Language language);
    }
}
