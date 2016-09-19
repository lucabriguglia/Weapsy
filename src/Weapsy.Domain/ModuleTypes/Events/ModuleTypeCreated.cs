using System;
using Weapsy.Core.Domain;

namespace Weapsy.Domain.ModuleTypes.Events
{
    public class ModuleTypeCreated : Event
    {
        public Guid? AppId { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ViewType ViewType { get; set; }
        public string ViewName { get; set; }
        public EditType EditType { get; set; }
        public string EditUrl { get; set; }
        public ModuleTypeStatus Status { get; set; }
    }
}
