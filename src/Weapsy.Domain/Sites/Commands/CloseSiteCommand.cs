using System;
using Weapsy.Framework.Commands;

namespace Weapsy.Domain.Sites.Commands
{
    public class CloseSiteCommand : ICommand
    {
        public Guid Id { get; set; }
    }
}
