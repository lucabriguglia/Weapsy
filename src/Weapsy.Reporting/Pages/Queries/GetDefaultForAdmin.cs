using System;
using Weapsy.Framework.Queries;

namespace Weapsy.Reporting.Pages.Queries
{
    public class GetDefaultForAdmin : IQuery
    {
        public Guid SiteId { get; set; }
    }
}
