using System;
using Weapsy.Cqrs.Domain;

namespace Weapsy.Domain.Users.Commands
{
    public class AddUserToRole : DomainCommand
    {
        public Guid Id { get; set; }
        public string RoleName { get; set; }
    }
}
