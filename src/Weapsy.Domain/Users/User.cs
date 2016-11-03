using FluentValidation;
using Weapsy.Infrastructure.Domain;
using Weapsy.Domain.Users.Commands;
using Weapsy.Domain.Users.Events;

namespace Weapsy.Domain.Users
{
    public class User : AggregateRoot
    {
        public string Email { get; private set; }
        public string UserName { get; private set; }
        public UserStatus Status { get; private set; }

        public User(){}

        private User(CreateUser cmd) : base(cmd.Id)
        {
            Email = cmd.Email;
            UserName = cmd.UserName;
            Status = UserStatus.Active;

            AddEvent(new UserCreated
            {
                AggregateRootId = Id,
                Email = Email,
                UserName = UserName,
                Status = Status
            });
        }

        public static User CreateNew(CreateUser cmd, IValidator<CreateUser> validator)
        {
            validator.ValidateCommand(cmd);

            return new User(cmd);
        }
    }
}
