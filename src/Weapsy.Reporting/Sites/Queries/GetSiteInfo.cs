using Kledex.Queries;

namespace Weapsy.Reporting.Models.Sites.Queries
{
    public class GetSiteInfo : CacheableQuery<SiteInfo>
    {
        public GetSiteInfo(string siteName)
        {
            SiteName = siteName;
            CacheKey = $"Weapsy | SiteInfo | {siteName}";
        }

        public string SiteName { get; set; }
    }
}
