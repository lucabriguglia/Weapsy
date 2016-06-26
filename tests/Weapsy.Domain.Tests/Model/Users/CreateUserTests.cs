using System;
using System.Linq;
using NUnit.Framework;
using Moq;
using FluentValidation;
using FluentValidation.Results;
using Weapsy.Domain.Model.Users;
using Weapsy.Domain.Model.Users.Commands;
using Weapsy.Domain.Model.Users.Events;

namespace Weapsy.Domain.Tests.Users
{
    [TestFixture]
    public class CreateUserTests
    {
        private CreateUser command;
        private Mock<IValidator<CreateUser>> validatorMock;
        private User user;
        private UserCreated @event;

        [SetUp]
        public void Setup()
        {
            command = new CreateUser
            {
                Id = Guid.NewGuid(),
                Email = "my@email.com",
                UserName = "my"
            };
            validatorMock = new Mock<IValidator<CreateUser>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());
            user = User.CreateNew(command, validatorMock.Object);
            @event = user.Events.OfType<UserCreated>().SingleOrDefault();
        }

        [Test]
        public void Should_validate_command()
        {
            validatorMock.Verify(x => x.Validate(command));
        }

        [Test]
        public void Should_set_id()
        {
            Assert.AreEqual(command.Id, user.Id);
        }

        [Test]
        public void Should_set_email()
        {
            Assert.AreEqual(command.Email, user.Email);
        }

        [Test]
        public void Should_set_user_name()
        {
            Assert.AreEqual(command.UserName, user.UserName);
        }

        [Test]
        public void Should_set_status_to_active()
        {
            Assert.AreEqual(UserStatus.Active, user.Status);
        }

        [Test]
        public void Should_add_user_created_event()
        {
            Assert.IsNotNull(@event);
        }

        [Test]
        public void Should_set_id_in_user_created_event()
        {
            Assert.AreEqual(user.Id, @event.AggregateRootId);
        }

        [Test]
        public void Should_set_email_in_user_created_event()
        {
            Assert.AreEqual(user.Email, @event.Email);
        }

        [Test]
        public void Should_set_user_name_in_user_created_event()
        {
            Assert.AreEqual(user.UserName, @event.UserName);
        }

        [Test]
        public void Should_set_status_in_user_created_event()
        {
            Assert.AreEqual(user.Status, @event.Status);
        }
    }
}
