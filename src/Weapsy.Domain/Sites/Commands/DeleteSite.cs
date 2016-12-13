using System;
using Weapsy.Infrastructure.Dispatcher;

namespace Weapsy.Domain.Sites.Commands
{
    public class DeleteSite : ICommand
    {
        public Guid Id { get; set; }
    }
}
