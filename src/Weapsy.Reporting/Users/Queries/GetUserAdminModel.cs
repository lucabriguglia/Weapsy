using System;
using Weapsy.Infrastructure.Queries;

namespace Weapsy.Reporting.Users.Queries
{
    public class GetUserAdminModel : IQuery
    {
        public Guid Id { get; set; }
    }
}
