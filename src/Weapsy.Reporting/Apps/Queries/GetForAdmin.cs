using System;
using Weapsy.Infrastructure.Queries;

namespace Weapsy.Reporting.Apps.Queries
{
    public class GetForAdmin : IQuery
    {
        public Guid Id { get; set; }
    }
}
