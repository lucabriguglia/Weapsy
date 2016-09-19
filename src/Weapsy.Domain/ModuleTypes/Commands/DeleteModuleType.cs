using System;
using Weapsy.Core.Domain;

namespace Weapsy.Domain.ModuleTypes.Commands
{
    public class DeleteModuleType : ICommand
    {
        public Guid Id { get; set; }
    }
}
