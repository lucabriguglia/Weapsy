using System;
using Weapsy.Framework.Queries;

namespace Weapsy.Reporting.Menus.Queries
{
    public class GetDefaultItemForAdmin : IQuery
    {
        public Guid SiteId { get; set; }
        public Guid MenuId { get; set; }
    }
}
