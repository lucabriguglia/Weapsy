using Weapsy.Core.Domain;

namespace Weapsy.Domain.Model.Users.Events
{
    public class UserCreated : Event
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public UserStatus Status { get; set; }
    }
}
