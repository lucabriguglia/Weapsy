using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Weapsy.Reporting.Pages
{
    public interface IPageFacade
    {
        PageViewModel GetPageViewModel(Guid siteId, Guid pageId);
        PageViewModel GetPageViewModel(Guid siteId, string name);
        Task<IEnumerable<PageAdminListModel>> GetAllForAdminAsync(Guid siteId);
        Task<PageAdminModel> GetAdminModelAsync(Guid siteId, Guid pageId);
        Task<PageAdminModel> GetDefaultAdminModelAsync(Guid siteId);
    }
}
