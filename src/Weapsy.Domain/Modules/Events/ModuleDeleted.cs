using System;
using Weapsy.Infrastructure.Domain;

namespace Weapsy.Domain.Modules.Events
{
    public class ModuleDeleted : Event
    {
        public Guid SiteId { get; set; }
    }
}
