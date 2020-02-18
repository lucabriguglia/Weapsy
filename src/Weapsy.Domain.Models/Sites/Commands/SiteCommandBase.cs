using System;
using Kledex.Commands;

namespace Weapsy.Domain.Models.Sites.Commands
{
    public abstract class SiteCommandBase : Command
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}