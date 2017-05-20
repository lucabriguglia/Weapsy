using Weapsy.Framework.Domain;

namespace Weapsy.Domain.Users.Events
{
    public class UserRegistered : DomainEvent
    {
        public string Email { get; set; }
        public string UserName { get; set; }
    }
}
