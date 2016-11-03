using System;
using Weapsy.Infrastructure.Domain;

namespace Weapsy.Domain.ModuleTypes.Commands
{
    public class DeleteModuleType : ICommand
    {
        public Guid Id { get; set; }
    }
}
