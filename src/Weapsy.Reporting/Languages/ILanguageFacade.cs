using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Weapsy.Reporting.Languages
{
    public interface ILanguageFacade
    {
        IEnumerable<LanguageInfo> GetAllActive(Guid siteId);
        Task<IEnumerable<LanguageAdminModel>> GetAllForAdminAsync(Guid siteId);
        Task<LanguageAdminModel> GetForAdminAsync(Guid siteId, Guid id);
    }
}
