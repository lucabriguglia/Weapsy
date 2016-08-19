using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Weapsy.Reporting.Pages
{
    public interface IPageFacade
    {
        PageInfo GetPageInfo(Guid siteId, Guid pageId, Guid languageId = new Guid());
        PageInfo GetPageInfo(Guid siteId, string name, Guid languageId = new Guid());
        Task<IEnumerable<PageAdminListModel>> GetAllForAdminAsync(Guid siteId);
        Task<PageAdminModel> GetAdminModelAsync(Guid siteId, Guid pageId);
        Task<PageAdminModel> GetDefaultAdminModelAsync(Guid siteId);
        Task<PageModuleAdminModel> GetModuleAdminModelAsync(Guid siteId, Guid pageId, Guid pageModuleId);
    }
}
