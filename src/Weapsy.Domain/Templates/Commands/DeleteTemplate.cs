using System;
using Weapsy.Infrastructure.Dispatcher;

namespace Weapsy.Domain.Templates.Commands
{
    public class DeleteTemplate : ICommand
    {
        public Guid Id { get; set; }
    }
}
