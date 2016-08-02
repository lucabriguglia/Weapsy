using System;
using Weapsy.Reporting.Pages;

namespace Weapsy.Reporting.Data.Default.Pages
{
    public interface IPageAdminFactory
    {
        PageAdminModel GetAdminModel(Guid siteId, Guid pageId);
        PageAdminModel GetDefaultAdminModel(Guid siteId);
    }
}