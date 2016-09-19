using System;
using Weapsy.Core.Domain;

namespace Weapsy.Domain.Model.Modules.Events
{
    public class ModuleCreated : Event
    {
        public Guid SiteId { get; set; }
        public Guid ModuleTypeId { get; set; }
        public string Title { get; set; }
        public ModuleStatus Status { get; set; }
    }
}
