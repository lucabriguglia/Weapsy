using System;
using Weapsy.Cqrs.Queries;

namespace Weapsy.Reporting.Apps.Queries
{
    public class GetAppAdminModel : IQuery
    {
        public Guid Id { get; set; }
    }
}
