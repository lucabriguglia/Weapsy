using System;
using Weapsy.Framework.Queries;

namespace Weapsy.Reporting.Apps.Queries
{
    public class GetModuleTypeAdminListModel : IQuery
    {
        public Guid AppId { get; set; }
    }
}
