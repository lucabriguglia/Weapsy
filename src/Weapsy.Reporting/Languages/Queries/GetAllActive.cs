using System;
using Weapsy.Cqrs.Queries;

namespace Weapsy.Reporting.Languages.Queries
{
    public class GetAllActive : IQuery
    {
        public Guid SiteId { get; set; }
    }
}
