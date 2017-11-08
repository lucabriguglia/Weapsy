using System;
using Weapsy.Framework.Commands;

namespace Weapsy.Domain.Sites.Commands
{
    public class ReopenSiteCommand : ICommand
    {
        public Guid Id { get; set; }
    }
}
