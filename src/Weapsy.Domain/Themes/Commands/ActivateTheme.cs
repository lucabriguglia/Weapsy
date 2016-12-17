using System;
using Weapsy.Infrastructure.Dispatcher;

namespace Weapsy.Domain.Themes.Commands
{
    public class ActivateTheme : ICommand
    {
        public Guid Id { get; set; }
    }
}
