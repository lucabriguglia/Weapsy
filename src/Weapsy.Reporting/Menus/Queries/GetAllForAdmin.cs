using System;
using Weapsy.Framework.Queries;

namespace Weapsy.Reporting.Menus.Queries
{
    public class GetAllForAdmin : IQuery
    {
        public Guid SiteId { get; set; }
    }
}
