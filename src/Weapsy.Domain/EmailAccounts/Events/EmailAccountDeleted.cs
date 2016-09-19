using System;
using Weapsy.Core.Domain;

namespace Weapsy.Domain.Model.EmailAccounts.Events
{
    public class EmailAccountDeleted : Event
    {
        public Guid SiteId { get; set; }
    }
}