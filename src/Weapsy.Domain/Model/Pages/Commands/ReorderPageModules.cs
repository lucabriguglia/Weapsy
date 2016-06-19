using System;
using System.Collections.Generic;
using Weapsy.Core.Domain;

namespace Weapsy.Domain.Model.Pages.Commands
{
    public class ReorderPageModules : ICommand
    {
        public Guid SiteId { get; set; }
        public Guid PageId { get; set; }
        public IEnumerable<Zone> Zones { get; set; } = new List<Zone>();

        public class Zone
        {
            public string Name { get; set; }
            public IList<Guid> Modules { get; set; } = new List<Guid>();
        }
    }
}
