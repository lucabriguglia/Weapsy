using System;
using Weapsy.Framework.Commands;

namespace Weapsy.Domain.Themes.Commands
{
    public class HideTheme : ICommand
    {
        public Guid Id { get; set; }
    }
}
