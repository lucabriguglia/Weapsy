using System;
using Weapsy.Reporting.Pages;

namespace Weapsy.Data.Reporting.Pages
{
    public interface IPageInfoFactory
    {
        PageInfo CreatePageInfo(Guid siteId, Guid pageId, Guid languageId = new Guid());
    }
}