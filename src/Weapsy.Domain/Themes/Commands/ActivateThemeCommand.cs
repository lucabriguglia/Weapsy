using System;
using Weapsy.Framework.Commands;

namespace Weapsy.Domain.Themes.Commands
{
    public class ActivateThemeCommand : ICommand
    {
        public Guid Id { get; set; }
    }
}
