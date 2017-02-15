using System;
using System.Collections.Generic;

namespace Weapsy.Reporting.Pages
{
    public interface IPageFacade
    {
        PageInfo GetPageInfo(Guid siteId, Guid pageId, Guid languageId = new Guid());
        PageInfo GetPageInfo(Guid siteId, string name, Guid languageId = new Guid());
        IEnumerable<PageAdminListModel> GetAllForAdmin(Guid siteId);
        PageAdminModel GetAdminModel(Guid siteId, Guid pageId);
        PageAdminModel GetDefaultAdminModel(Guid siteId);
        PageModuleAdminModel GetModuleAdminModel(Guid siteId, Guid pageId, Guid pageModuleId);
        Guid? GetIdBySlug(Guid siteId, string slug);
        Guid? GetIdBySlug(Guid siteId, string slug, Guid languageId);
    }
}
