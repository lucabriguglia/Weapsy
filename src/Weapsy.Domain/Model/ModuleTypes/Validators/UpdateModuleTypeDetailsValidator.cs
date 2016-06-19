using Weapsy.Domain.Model.ModuleTypes.Commands;
using Weapsy.Domain.Model.ModuleTypes.Rules;

namespace Weapsy.Domain.Model.ModuleTypes.Validators
{
    public class UpdateModuleTypeDetailsValidator : ModuleTypeDetailsValidator<UpdateModuleTypeDetails>
    {
        public UpdateModuleTypeDetailsValidator(IModuleTypeRules moduleTypeRules) : base(moduleTypeRules) {}
    }
}