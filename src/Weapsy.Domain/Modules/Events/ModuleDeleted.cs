using System;
using Weapsy.Infrastructure.Domain;

namespace Weapsy.Domain.Modules.Events
{
    public class ModuleDeleted : DomainEvent
    {
        public Guid SiteId { get; set; }
    }
}
