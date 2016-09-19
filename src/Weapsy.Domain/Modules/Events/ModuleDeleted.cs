using System;
using Weapsy.Core.Domain;

namespace Weapsy.Domain.Modules.Events
{
    public class ModuleDeleted : Event
    {
        public Guid SiteId { get; set; }
    }
}
