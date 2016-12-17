using System;
using Weapsy.Infrastructure.Dispatcher;

namespace Weapsy.Domain.Users.Commands
{
    public class CreateUser : ICommand
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
    }
}
