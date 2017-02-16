using System;
using Weapsy.Infrastructure.Queries;

namespace Weapsy.Reporting.ModuleTypes.Queries
{
    public class GetAllForAdmin : IQuery
    {
        public Guid AppId { get; set; }
    }
}
