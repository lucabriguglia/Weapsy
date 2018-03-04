using Weapsy.Cqrs.Queries;

namespace Weapsy.Reporting.Sites.Queries
{
    public class IsSiteInstalled : IQuery
    {
        public string Name { get; set; }
    }
}
