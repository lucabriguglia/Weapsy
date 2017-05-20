using System;
using System.Collections.Generic;
using Weapsy.Framework.Commands;

namespace Weapsy.Domain.Themes.Commands
{
    public class ReorderThemes : ICommand
    {
        public IList<Guid> Themes { get; set; }
    }
}
