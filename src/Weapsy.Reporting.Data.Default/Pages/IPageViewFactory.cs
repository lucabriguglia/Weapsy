using System;
using Weapsy.Reporting.Pages;

namespace Weapsy.Reporting.Data.Default.Pages
{
    public interface IPageViewFactory
    {
        PageViewModel GetPageViewModel(Guid siteId, Guid pageId, Guid languageId = new Guid());
    }
}