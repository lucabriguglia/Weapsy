using System;
using Weapsy.Core.Domain;

namespace Weapsy.Domain.Model.Sites.Rules
{
    public interface ISiteRules : IRules<Site>
    {
        bool DoesSiteExist(Guid id);
        bool IsSiteIdUnique(Guid id);
        bool IsSiteNameValid(string name);
        bool IsSiteNameUnique(string name);
        bool IsSiteUrlValid(string url);
        bool IsSiteUrlUnique(string url, Guid siteId = new Guid());
    }
}
