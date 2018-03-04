using System;
using Weapsy.Cqrs.Queries;

namespace Weapsy.Reporting.Menus.Queries
{
    public class GetAllForAdmin : IQuery
    {
        public Guid SiteId { get; set; }
    }
}
