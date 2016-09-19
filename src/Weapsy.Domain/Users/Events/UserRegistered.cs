using Weapsy.Core.Domain;

namespace Weapsy.Domain.Users.Events
{
    public class UserRegistered : Event
    {
        public string Email { get; set; }
        public string UserName { get; set; }
    }
}
