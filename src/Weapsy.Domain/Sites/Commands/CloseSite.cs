using System;
using Weapsy.Framework.Commands;

namespace Weapsy.Domain.Sites.Commands
{
    public class CloseSite : ICommand
    {
        public Guid Id { get; set; }
    }
}
