using System;
using Weapsy.Infrastructure.Domain;

namespace Weapsy.Domain.Themes.Commands
{
    public class HideTheme : ICommand
    {
        public Guid Id { get; set; }
    }
}
