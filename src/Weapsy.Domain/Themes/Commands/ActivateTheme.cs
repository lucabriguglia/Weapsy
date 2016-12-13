using System;
using Weapsy.Infrastructure.Dispatcher;
using Weapsy.Infrastructure.Domain;

namespace Weapsy.Domain.Themes.Commands
{
    public class ActivateTheme : ICommand
    {
        public Guid Id { get; set; }
    }
}
