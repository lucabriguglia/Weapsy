using System;
using Weapsy.Framework.Commands;

namespace Weapsy.Domain.Users.Commands
{
    public class AddUserToRole : ICommand
    {
        public Guid Id { get; set; }
        public string RoleName { get; set; }
    }
}
