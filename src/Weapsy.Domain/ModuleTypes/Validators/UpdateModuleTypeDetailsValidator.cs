using Weapsy.Domain.ModuleTypes.Commands;
using Weapsy.Domain.ModuleTypes.Rules;

namespace Weapsy.Domain.ModuleTypes.Validators
{
    public class UpdateModuleTypeDetailsValidator : ModuleTypeDetailsValidator<UpdateModuleTypeDetailsCommand>
    {
        public UpdateModuleTypeDetailsValidator(IModuleTypeRules moduleTypeRules) : base(moduleTypeRules) {}
    }
}