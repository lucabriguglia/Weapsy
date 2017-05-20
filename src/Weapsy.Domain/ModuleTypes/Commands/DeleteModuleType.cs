using System;
using Weapsy.Framework.Commands;

namespace Weapsy.Domain.ModuleTypes.Commands
{
    public class DeleteModuleType : ICommand
    {
        public Guid Id { get; set; }
    }
}
