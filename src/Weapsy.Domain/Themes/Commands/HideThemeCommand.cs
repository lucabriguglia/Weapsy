using System;
using Weapsy.Framework.Commands;

namespace Weapsy.Domain.Themes.Commands
{
    public class HideThemeCommand : ICommand
    {
        public Guid Id { get; set; }
    }
}
