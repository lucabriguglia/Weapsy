using System;
using Weapsy.Infrastructure.Domain;

namespace Weapsy.Domain.Users.Events
{
    public class UserRemovedFromRole : DomainEvent
    {
        public string RoleName { get; set; }
    }
}
