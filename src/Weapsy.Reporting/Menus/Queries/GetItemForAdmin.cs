using System;
using Weapsy.Framework.Queries;

namespace Weapsy.Reporting.Menus.Queries
{
    public class GetItemForAdmin : IQuery
    {
        public Guid SiteId { get; set; }
        public Guid MenuId { get; set; }
        public Guid MenuItemId { get; set; }
    }
}
