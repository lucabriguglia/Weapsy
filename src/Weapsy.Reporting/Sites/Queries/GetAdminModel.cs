using System;
using Weapsy.Infrastructure.Queries;

namespace Weapsy.Reporting.Sites.Queries
{
    public class GetAdminModel : IQuery
    {
        public Guid Id { get; set; }
    }
}
