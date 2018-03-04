using System;
using Weapsy.Cqrs.Queries;

namespace Weapsy.Reporting.Apps.Queries
{
    public class GetModuleTypeAdminListModel : IQuery
    {
        public Guid AppId { get; set; }
    }
}
