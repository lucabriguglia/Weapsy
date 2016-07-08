using System;
using System.Collections.Generic;
using FluentValidation;
using Weapsy.Core.Domain;
using Weapsy.Domain.Model.Roles.Commands;

namespace Weapsy.Domain.Model.Roles.Handlers
{
    public class CreateRoleHandler : ICommandHandler<CreateRole>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IValidator<CreateRole> _validator;

        public CreateRoleHandler(IRoleRepository roleRepository, IValidator<CreateRole> validator)
        {
            _roleRepository = roleRepository;
            _validator = validator;
        }

        public ICollection<IEvent> Handle(CreateRole command)
        {
            throw new NotImplementedException();
        }
    }
}
