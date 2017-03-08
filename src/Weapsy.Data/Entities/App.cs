using System;
using System.Collections.Generic;
using Weapsy.Domain.Apps;

namespace Weapsy.Data.Entities
{
    public class App
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Folder { get; set; }
        public AppStatus Status { get; set; }

        public virtual ICollection<ModuleType> ModuleTypes { get; set; }
    }
}