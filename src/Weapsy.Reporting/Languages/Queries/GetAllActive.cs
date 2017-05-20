using System;
using Weapsy.Framework.Queries;

namespace Weapsy.Reporting.Languages.Queries
{
    public class GetAllActive : IQuery
    {
        public Guid SiteId { get; set; }
    }
}
