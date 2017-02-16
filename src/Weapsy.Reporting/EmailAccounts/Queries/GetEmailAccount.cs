using System;
using Weapsy.Infrastructure.Queries;

namespace Weapsy.Reporting.EmailAccounts.Queries
{
    public class GetEmailAccount : IQuery
    {
        public Guid SiteId { get; set; }
        public Guid Id { get; set; }
    }
}
