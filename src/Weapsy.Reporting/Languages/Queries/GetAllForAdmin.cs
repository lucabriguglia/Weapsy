using System;
using Weapsy.Infrastructure.Queries;

namespace Weapsy.Reporting.Languages.Queries
{
    public class GetAllForAdmin : IQuery
    {
        public Guid SiteId { get; set; }
    }
}
