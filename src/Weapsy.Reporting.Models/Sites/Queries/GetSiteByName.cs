using Kledex.Queries;

namespace Weapsy.Reporting.Models.Sites.Queries
{
    public class GetSiteByName : IQuery<SiteModel>
    {
        public string SiteName { get; set; }
    }
}
