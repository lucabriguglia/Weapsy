using System;
using System.Collections.Generic;
using Weapsy.Domain.ModuleTypes;

namespace Weapsy.Data.Entities
{
    public class ModuleType
    {
        public Guid AppId { get; set; }
        public Guid Id { get; set; }        
        public string Name { get; set; }        
        public string Title { get; set; }
        public string Description { get; set; }
        public ViewType ViewType { get; set; }
        public string ViewName { get; set; }
        public EditType EditType { get; set; }
        public string EditUrl { get; set; }
        public ModuleTypeStatus Status { get; set; }

        public virtual App App { get; set; }
        public virtual ICollection<Module> Modules { get; set; } = new List<Module>();
    }
}