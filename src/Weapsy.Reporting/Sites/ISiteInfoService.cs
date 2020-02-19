using System.Threading.Tasks;
using Weapsy.Reporting.Sites.Models;
using Weapsy.Reporting.Sites.Queries;

namespace Weapsy.Reporting.Sites
{
    public interface ISiteInfoService
    {
        Task<SiteInfo> GetSiteInfoAsync(GetSiteInfo query);
    }
}