using System;
using Weapsy.Infrastructure.Commands;

namespace Weapsy.Domain.Users.Commands
{
    public class RemoveUserFromRole : ICommand
    {
        public Guid Id { get; set; }
        public string RoleName { get; set; }
    }
}
