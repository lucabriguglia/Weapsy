using System;
using Weapsy.Infrastructure.Dispatcher;

namespace Weapsy.Domain.Templates.Commands
{
    public class ActivateTemplate : ICommand
    {
        public Guid Id { get; set; }
    }
}
