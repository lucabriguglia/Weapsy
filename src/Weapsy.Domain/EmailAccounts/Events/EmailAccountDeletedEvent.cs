using System;
using Weapsy.Framework.Domain;

namespace Weapsy.Domain.EmailAccounts.Events
{
    public class EmailAccountDeletedEvent : DomainEvent
    {
        public Guid SiteId { get; set; }
    }
}