using System;

namespace Weapsy.Domain.ModuleTypes.Commands
{
    public class CreateModuleTypeCommand : ModuleTypeDetailsCommand
    {
        public Guid AppId { get; set; }
    }
}
