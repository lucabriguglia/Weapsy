using System;
using Weapsy.Infrastructure.Queries;

namespace Weapsy.Reporting.Pages.Queries
{
    public class GetAllForAdmin : IQuery
    {
        public Guid SiteId { get; set; }
    }
}
