using System;
using Weapsy.Core.Domain;

namespace Weapsy.Domain.Model.Users.Commands
{
    public class CreateUser : ICommand
    {
        public Guid Id { get; set; }
    }
}
