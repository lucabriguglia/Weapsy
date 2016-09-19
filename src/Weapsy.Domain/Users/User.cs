using FluentValidation;
using Weapsy.Core.Domain;
using Weapsy.Domain.Model.Users.Commands;
using Weapsy.Domain.Model.Users.Events;

namespace Weapsy.Domain.Model.Users
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
