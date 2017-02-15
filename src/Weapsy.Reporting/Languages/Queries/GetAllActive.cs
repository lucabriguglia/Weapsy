using System;
using Weapsy.Infrastructure.Queries;

namespace Weapsy.Reporting.Languages.Queries
{
    public class GetAllActive : IQuery
    {
        public Guid SiteId { get; set; }
    }
}
