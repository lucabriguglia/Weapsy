using System;

namespace Weapsy.Domain.ModuleTypes.Commands
{
    public class CreateModuleType : ModuleTypeDetails
    {
        public Guid AppId { get; set; }
    }
}
