using FluentValidation;
using Weapsy.Core.Domain;
using Weapsy.Domain.Model.Users.Commands;
using Weapsy.Domain.Model.Users.Events;

namespace Weapsy.Domain.Model.Users
{
    public class User : AggregateRoot
    {
        public User(){}

        private User(CreateUser cmd) : base(cmd.Id)
        {
            AddEvent(new UserCreated
            {
                AggregateRootId = Id
            });
        }

        public static User CreateNew(CreateUser cmd, IValidator<CreateUser> validator)
        {
            validator.ValidateCommand(cmd);

            return new User(cmd);
        }
    }
}
