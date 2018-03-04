using System;
using Weapsy.Cqrs.Domain;

namespace Weapsy.Domain.Users.Commands
{
    public class RemoveUserFromRole : DomainCommand
    {
        public Guid Id { get; set; }
        public string RoleName { get; set; }
    }
}
