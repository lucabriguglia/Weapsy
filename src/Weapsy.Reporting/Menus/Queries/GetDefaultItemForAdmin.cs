using System;
using Weapsy.Cqrs.Queries;

namespace Weapsy.Reporting.Menus.Queries
{
    public class GetDefaultItemForAdmin : IQuery
    {
        public Guid SiteId { get; set; }
        public Guid MenuId { get; set; }
    }
}
