using System;
using Weapsy.Framework.Domain;

namespace Weapsy.Domain.Users.Events
{
    public class UserAddedToRoleEvent : DomainEvent
    {
        public string RoleName { get; set; }
    }
}
