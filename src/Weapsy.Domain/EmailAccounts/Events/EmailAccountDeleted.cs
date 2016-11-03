using System;
using Weapsy.Infrastructure.Domain;

namespace Weapsy.Domain.EmailAccounts.Events
{
    public class EmailAccountDeleted : Event
    {
        public Guid SiteId { get; set; }
    }
}