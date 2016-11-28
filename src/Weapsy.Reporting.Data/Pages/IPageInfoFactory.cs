using System;
using Weapsy.Reporting.Pages;

namespace Weapsy.Reporting.Data.Pages
{
    public interface IPageInfoFactory
    {
        PageInfo CreatePageInfo(Guid siteId, Guid pageId, Guid languageId = new Guid());
    }
}