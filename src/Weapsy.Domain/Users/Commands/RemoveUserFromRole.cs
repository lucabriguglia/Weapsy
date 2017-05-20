using System;
using Weapsy.Framework.Commands;

namespace Weapsy.Domain.Users.Commands
{
    public class RemoveUserFromRole : ICommand
    {
        public Guid Id { get; set; }
        public string RoleName { get; set; }
    }
}
