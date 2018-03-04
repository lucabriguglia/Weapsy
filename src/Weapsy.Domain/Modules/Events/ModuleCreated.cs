using System;
using Weapsy.Cqrs.Domain;
using Weapsy.Framework.Domain;

namespace Weapsy.Domain.Modules.Events
{
    public class ModuleCreated : DomainEvent
    {
        public Guid SiteId { get; set; }
        public Guid ModuleTypeId { get; set; }
        public string Title { get; set; }
        public ModuleStatus Status { get; set; }
    }
}
