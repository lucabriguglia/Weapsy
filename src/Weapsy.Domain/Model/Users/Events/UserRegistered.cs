using Weapsy.Core.Domain;

namespace Weapsy.Domain.Model.Users.Events
{
    public class UserRegistered : Event
    {
        public string Email { get; set; }
        public string UserName { get; set; }
    }
}
