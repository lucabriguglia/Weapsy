using System;
using Weapsy.Framework.Queries;

namespace Weapsy.Reporting.Languages.Queries
{
    public class GetAllForAdmin : IQuery
    {
        public Guid SiteId { get; set; }
    }
}
