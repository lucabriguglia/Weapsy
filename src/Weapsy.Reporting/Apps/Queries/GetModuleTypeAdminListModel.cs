using System;
using Weapsy.Infrastructure.Queries;

namespace Weapsy.Reporting.Apps.Queries
{
    public class GetModuleTypeAdminListModel : IQuery
    {
        public Guid AppId { get; set; }
    }
}
