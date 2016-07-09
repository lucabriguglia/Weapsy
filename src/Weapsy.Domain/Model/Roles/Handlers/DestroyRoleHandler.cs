using System;
using System.Collections.Generic;
using FluentValidation;
using Weapsy.Core.Domain;
using Weapsy.Domain.Model.Roles.Commands;
using Weapsy.Domain.Model.Roles.Events;

namespace Weapsy.Domain.Model.Roles.Handlers
{
    public class DestroyRoleHandler : ICommandHandler<DestroyRole>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IValidator<DestroyRole> _validator;

        public DestroyRoleHandler(IRoleRepository roleRepository, IValidator<DestroyRole> validator)
        {
            _roleRepository = roleRepository;
            _validator = validator;
        }

        public ICollection<IEvent> Handle(DestroyRole command)
        {
            var role = _roleRepository.GetById(command.Id);

            if (role == null)
                throw new Exception($"Role {command.Name} not found");

            _roleRepository.Delete(role);

            return new List<IEvent>
            {
                new RoleDestroyed
                {
                    AggregateRootId = role.Id,
                    Name = role.Name
                }
            };
        }
    }
}
