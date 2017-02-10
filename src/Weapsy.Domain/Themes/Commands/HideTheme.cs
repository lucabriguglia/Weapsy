using System;
using Weapsy.Infrastructure.Commands;

namespace Weapsy.Domain.Themes.Commands
{
    public class HideTheme : ICommand
    {
        public Guid Id { get; set; }
    }
}
