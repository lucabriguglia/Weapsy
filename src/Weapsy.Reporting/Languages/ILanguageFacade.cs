using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Weapsy.Reporting.Languages
{
    public interface ILanguageFacade
    {
        IEnumerable<LanguageInfo> GetAllActive(Guid siteId);
        IEnumerable<LanguageAdminModel> GetAllForAdmin(Guid siteId);
        LanguageAdminModel GetForAdmin(Guid siteId, Guid id);
    }
}
