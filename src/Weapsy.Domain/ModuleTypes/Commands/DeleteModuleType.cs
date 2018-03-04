using System;
using Weapsy.Cqrs.Domain;

namespace Weapsy.Domain.ModuleTypes.Commands
{
    public class DeleteModuleType : DomainCommand
    {
        public Guid Id { get; set; }
    }
}
