using Kledex.Queries;

namespace Weapsy.Reporting.Models.Sites.Queries
{
    public class GetSiteInfo : IQuery<SiteInfo>
    {
        public string SiteName { get; set; }
    }
}
