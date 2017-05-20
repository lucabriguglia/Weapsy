using System;
using Weapsy.Framework.Domain;

namespace Weapsy.Domain.Sites.Rules
{
    public interface ISiteRules : IRules<Site>
    {
        bool DoesSiteExist(Guid id);
        bool IsSiteIdUnique(Guid id);
        bool IsSiteNameValid(string name);
        bool IsSiteNameUnique(string name);
        bool IsSiteUrlValid(string url);
        bool IsSiteUrlUnique(string url, Guid siteId = new Guid());
        bool IsPageSetAsHomePage(Guid siteId, Guid pageId);
    }
}
