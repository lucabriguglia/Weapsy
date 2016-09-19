using System;
using Weapsy.Core.Domain;

namespace Weapsy.Domain.EmailAccounts.Events
{
    public class EmailAccountDeleted : Event
    {
        public Guid SiteId { get; set; }
    }
}