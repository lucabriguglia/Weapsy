using System;
using Weapsy.Framework.Commands;

namespace Weapsy.Domain.Apps.Commands
{
    public class AppDetailsCommand : ICommand
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Folder { get; set; }
    }
}
