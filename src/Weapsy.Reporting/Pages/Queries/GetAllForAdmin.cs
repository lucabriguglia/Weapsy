using System;
using Weapsy.Cqrs.Queries;

namespace Weapsy.Reporting.Pages.Queries
{
    public class GetAllForAdmin : IQuery
    {
        public Guid SiteId { get; set; }
    }
}
