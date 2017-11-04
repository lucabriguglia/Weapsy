using System;
using Weapsy.Framework.Commands;

namespace Weapsy.Domain.Themes.Commands
{
    public class DeleteThemeCommand : ICommand
    {
        public Guid Id { get; set; }
    }
}
