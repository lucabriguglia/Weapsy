using Weapsy.Framework.Domain;

namespace Weapsy.Domain.Users.Events
{
    public class UserCreated : DomainEvent
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public UserStatus Status { get; set; }
    }
}
