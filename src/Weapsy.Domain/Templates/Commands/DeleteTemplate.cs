using System;
using Weapsy.Infrastructure.Dispatcher;
using Weapsy.Infrastructure.Domain;

namespace Weapsy.Domain.Templates.Commands
{
    public class DeleteTemplate : ICommand
    {
        public Guid Id { get; set; }
    }
}
