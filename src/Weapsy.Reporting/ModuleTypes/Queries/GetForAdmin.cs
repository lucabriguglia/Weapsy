using System;
using Weapsy.Infrastructure.Queries;

namespace Weapsy.Reporting.ModuleTypes.Queries
{
    public class GetForAdmin : IQuery
    {
        public Guid AppId { get; set; }
        public Guid Id { get; set; }
    }
}
