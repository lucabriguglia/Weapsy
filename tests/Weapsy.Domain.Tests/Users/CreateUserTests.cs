using System;
using System.Linq;
using NUnit.Framework;
using Moq;
using FluentValidation;
using FluentValidation.Results;
using Weapsy.Domain.Users;
using Weapsy.Domain.Users.Commands;
using Weapsy.Domain.Users.Events;

namespace Weapsy.Domain.Tests.Users
{
    [TestFixture]
    public class CreateUserTests
    {
        private CreateUser _command;
        private Mock<IValidator<CreateUser>> _validatorMock;
        private User _user;
        private UserCreated _event;

        [SetUp]
        public void Setup()
        {
            _command = new CreateUser
            {
                Id = Guid.NewGuid(),
                Email = "my@email.com",
                UserName = "my"
            };
            _validatorMock = new Mock<IValidator<CreateUser>>();
            _validatorMock.Setup(x => x.Validate(_command)).Returns(new ValidationResult());
            _user = User.CreateNew(_command, _validatorMock.Object);
            _event = _user.Events.OfType<UserCreated>().SingleOrDefault();
        }

        [Test]
        public void Should_validate_command()
        {
            _validatorMock.Verify(x => x.Validate(_command));
        }

        [Test]
        public void Should_set_id()
        {
            Assert.AreEqual(_command.Id, _user.Id);
        }

        [Test]
        public void Should_set_email()
        {
            Assert.AreEqual(_command.Email, _user.Email);
        }

        [Test]
        public void Should_set_user_name()
        {
            Assert.AreEqual(_command.UserName, _user.UserName);
        }

        [Test]
        public void Should_set_status_to_active()
        {
            Assert.AreEqual(UserStatus.Active, _user.Status);
        }

        [Test]
        public void Should_add_user_created_event()
        {
            Assert.IsNotNull(_event);
        }

        [Test]
        public void Should_set_id_in_user_created_event()
        {
            Assert.AreEqual(_user.Id, _event.AggregateRootId);
        }

        [Test]
        public void Should_set_email_in_user_created_event()
        {
            Assert.AreEqual(_user.Email, _event.Email);
        }

        [Test]
        public void Should_set_user_name_in_user_created_event()
        {
            Assert.AreEqual(_user.UserName, _event.UserName);
        }

        [Test]
        public void Should_set_status_in_user_created_event()
        {
            Assert.AreEqual(_user.Status, _event.Status);
        }
    }
}
