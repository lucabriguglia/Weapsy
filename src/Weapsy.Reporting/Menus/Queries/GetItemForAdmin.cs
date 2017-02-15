using System;
using Weapsy.Infrastructure.Queries;

namespace Weapsy.Reporting.Menus.Queries
{
    public class GetItemForAdmin : IQuery
    {
        public Guid SiteId { get; set; }
        public Guid MenuId { get; set; }
        public Guid MenuItemId { get; set; }
    }
}
