using System;
using Weapsy.Framework.Domain;

namespace Weapsy.Domain.Modules.Events
{
    public class ModuleDeletedEvent : DomainEvent
    {
        public Guid SiteId { get; set; }
    }
}
