using System;
using System.Collections.Generic;
using Weapsy.Core.Domain;

namespace Weapsy.Domain.Languages.Commands
{
    public class ReorderLanguages : ICommand
    {
        public Guid SiteId { get; set; }
        public IList<Guid> Languages { get; set; }
    }
}
