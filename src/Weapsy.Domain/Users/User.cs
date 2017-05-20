using System.Collections.Generic;
using FluentValidation;
using Weapsy.Framework.Domain;
using Weapsy.Domain.Users.Commands;
using Weapsy.Domain.Users.Events;

namespace Weapsy.Domain.Users
{
    public class User : AggregateRoot
    {
        public string Email { get; private set; }
        public string UserName { get; private set; }
        public string DisplayName { get; private set; }
        public string Prefix { get; private set; }
        public string FirstName { get; private set; }
        public string MiddleNames { get; private set; }
        public string Surname { get; private set; }
        public UserStatus Status { get; private set; }

        public IList<Address> Addresses { get; private set; } = new List<Address>();
        public IList<ContactNumber> ContactNumbers { get; private set; } = new List<ContactNumber>();

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
