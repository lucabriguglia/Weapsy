using System;
using System.Threading.Tasks;

namespace Weapsy.Reporting.Sites
{
    public interface ISiteFacade
    {
        Task<SiteInfo> GetSiteInfo(string name);
        Task<SiteAdminModel> GetAdminModel(Guid id);
    }
}
