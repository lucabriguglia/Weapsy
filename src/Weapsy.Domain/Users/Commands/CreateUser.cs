using System;
using Weapsy.Cqrs.Domain;

namespace Weapsy.Domain.Users.Commands
{
    public class CreateUser : DomainCommand
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
    }
}
