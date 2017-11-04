using System;
using Weapsy.Framework.Commands;

namespace Weapsy.Domain.Sites.Commands
{
    public class CreateSiteCommand : ICommand
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
