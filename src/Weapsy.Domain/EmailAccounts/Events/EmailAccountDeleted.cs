using System;
using Weapsy.Cqrs.Domain;

namespace Weapsy.Domain.EmailAccounts.Events
{
    public class EmailAccountDeleted : DomainEvent
    {
        public Guid SiteId { get; set; }
    }
}