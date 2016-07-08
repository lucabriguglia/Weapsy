using FluentValidation;
using Weapsy.Core.Domain;
using Weapsy.Domain.Model.Roles.Commands;
using Weapsy.Domain.Model.Roles.Events;

namespace Weapsy.Domain.Model.Roles
{
    public class Role : AggregateRoot
    {
        public string Name { get; set; }

        public Role() { }

        private Role(CreateRole cmd) : base(cmd.Id)
        {
            Name = cmd.Name;

            AddEvent(new RoleCreated
            {
                AggregateRootId = Id,
                Name = Name                
            });
        }

        public static Role CreateNew(CreateRole cmd, IValidator<CreateRole> validator)
        {
            validator.ValidateCommand(cmd);

            return new Role(cmd);
        }
    }
}
