using System;
using Weapsy.Infrastructure.Commands;

namespace Weapsy.Domain.Users.Commands
{
    public class AddUserToRole : ICommand
    {
        public Guid Id { get; set; }
        public string RoleName { get; set; }
    }
}
