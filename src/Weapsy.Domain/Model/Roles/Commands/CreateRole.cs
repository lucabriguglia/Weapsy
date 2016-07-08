using System;
using Weapsy.Core.Domain;

namespace Weapsy.Domain.Model.Roles.Commands
{
    public class CreateRole : ICommand
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
