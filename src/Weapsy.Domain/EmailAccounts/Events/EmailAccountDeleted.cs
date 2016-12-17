using System;
using Weapsy.Infrastructure.Domain;

namespace Weapsy.Domain.EmailAccounts.Events
{
    public class EmailAccountDeleted : DomainEvent
    {
        public Guid SiteId { get; set; }
    }
}