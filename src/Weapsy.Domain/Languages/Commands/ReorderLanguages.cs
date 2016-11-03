using System;
using System.Collections.Generic;

namespace Weapsy.Domain.Languages.Commands
{
    public class ReorderLanguages : BaseSiteCommand
    {
        public IList<Guid> Languages { get; set; }
    }
}
