using System;
using Weapsy.Infrastructure.Dispatcher;
using Weapsy.Infrastructure.Domain;

namespace Weapsy.Domain.Sites.Commands
{
    public class CloseSite : ICommand
    {
        public Guid Id { get; set; }
    }
}
