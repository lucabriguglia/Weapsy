using System;
using Weapsy.Cqrs.Queries;

namespace Weapsy.Reporting.Users.Queries
{
    public class GetUserAdminModel : IQuery
    {
        public Guid Id { get; set; }
    }
}
