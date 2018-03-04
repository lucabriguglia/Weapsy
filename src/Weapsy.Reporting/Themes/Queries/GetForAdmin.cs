using System;
using Weapsy.Cqrs.Queries;

namespace Weapsy.Reporting.Themes.Queries
{
    public class GetForAdmin : IQuery
    {
        public Guid Id { get; set; }
    }
}
