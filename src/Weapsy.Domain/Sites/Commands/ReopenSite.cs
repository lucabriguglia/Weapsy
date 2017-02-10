using System;
using Weapsy.Infrastructure.Commands;

namespace Weapsy.Domain.Sites.Commands
{
    public class ReopenSite : ICommand
    {
        public Guid Id { get; set; }
    }
}
