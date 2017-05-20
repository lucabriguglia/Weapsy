using System;
using Weapsy.Framework.Domain;

namespace Weapsy.Domain.Modules.Events
{
    public class ModuleDeleted : DomainEvent
    {
        public Guid SiteId { get; set; }
    }
}
