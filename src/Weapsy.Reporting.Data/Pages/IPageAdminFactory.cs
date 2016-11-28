using System;
using Weapsy.Reporting.Pages;

namespace Weapsy.Reporting.Data.Pages
{
    public interface IPageAdminFactory
    {
        PageAdminModel GetAdminModel(Guid siteId, Guid pageId);
        PageAdminModel GetDefaultAdminModel(Guid siteId);
    }
}