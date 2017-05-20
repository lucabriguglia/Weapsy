using System;
using Weapsy.Framework.Queries;

namespace Weapsy.Reporting.EmailAccounts.Queries
{
    public class GetAllEmailAccounts : IQuery
    {
        public Guid SiteId { get; set; }
    }
}
