using System;
using Weapsy.Infrastructure.Dispatcher;

namespace Weapsy.Domain.Sites.Commands
{
    public class CreateSite : ICommand
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
