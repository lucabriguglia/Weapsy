using System;
using Weapsy.Cqrs.Queries;

namespace Weapsy.Reporting.Pages.Queries
{
    public class GetForAdmin : IQuery
    {
        public Guid SiteId { get; set; }
        public Guid Id { get; set; }
    }
}
