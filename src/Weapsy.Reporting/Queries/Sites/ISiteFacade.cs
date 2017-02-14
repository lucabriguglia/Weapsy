using System;

namespace Weapsy.Reporting.Sites
{
    public interface ISiteFacade
    {
        SiteInfo GetSiteInfo(string name, Guid languageId = new Guid());
        SiteAdminModel GetAdminModel(Guid id);
    }
}
