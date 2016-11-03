using System;
using System.Collections.Generic;

namespace Weapsy.Domain.Pages.Commands
{
    public class ReorderPageModules : BaseSiteCommand
    {
        public Guid PageId { get; set; }
        public IEnumerable<Zone> Zones { get; set; } = new List<Zone>();

        public class Zone
        {
            public string Name { get; set; }
            public IList<Guid> Modules { get; set; } = new List<Guid>();
        }
    }
}
