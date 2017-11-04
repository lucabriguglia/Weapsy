using System;
using Weapsy.Framework.Commands;

namespace Weapsy.Domain.ModuleTypes.Commands
{
    public class DeleteModuleTypeCommand : ICommand
    {
        public Guid Id { get; set; }
    }
}
