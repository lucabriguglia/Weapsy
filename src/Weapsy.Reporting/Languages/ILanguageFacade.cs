using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Weapsy.Reporting.Languages
{
    public interface ILanguageFacade
    {
        Task<IEnumerable<LanguageInfo>> GetAllActive(Guid siteId);
        Task<IEnumerable<LanguageAdminModel>> GetAllForAdmin(Guid siteId);
        Task<LanguageAdminModel> GetForAdmin(Guid siteId, Guid id);
    }
}
