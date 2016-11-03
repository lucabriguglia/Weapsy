using System;
using Weapsy.Infrastructure.Domain;

namespace Weapsy.Domain.ModuleTypes.Commands
{
    public class CreateModuleType : ModuleTypeDetails
    {
        public Guid AppId { get; set; }
    }
}
