using System;
using Weapsy.Infrastructure.Dispatcher;

namespace Weapsy.Domain.Sites.Commands
{
    public class CloseSite : ICommand
    {
        public Guid Id { get; set; }
    }
}
