using System;
using Weapsy.Framework.Queries;

namespace Weapsy.Reporting.Apps.Queries
{
    public class GetModuleTypeAdminModel : IQuery
    {
        public Guid AppId { get; set; }
        public Guid Id { get; set; }
    }
}
