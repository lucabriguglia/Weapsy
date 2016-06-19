using System;
using System.Collections.Generic;
using Weapsy.Core.Domain;

namespace Weapsy.Domain.Model.Themes.Commands
{
    public class ReorderThemes : ICommand
    {
        public IList<Guid> Themes { get; set; }
    }
}
