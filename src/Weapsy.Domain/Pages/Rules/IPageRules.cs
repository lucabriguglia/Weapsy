using System;
using Weapsy.Core.Domain;

namespace Weapsy.Domain.Pages.Rules
{
    public interface IPageRules : IRules<Page>
    {
        bool DoesPageExist(Guid id);
        bool DoesPageExist(Guid siteId, Guid pageId);
        bool IsPageIdUnique(Guid id);
        bool IsPageNameValid(string name);
        bool IsPageNameUnique(Guid siteId, string name, Guid pageId = new Guid());
        bool IsPageUrlValid(string url);
        bool IsPageUrlReserved(string url);
        bool IsPageUrlUnique(Guid siteId, string url, Guid pageId = new Guid());
    }
}
