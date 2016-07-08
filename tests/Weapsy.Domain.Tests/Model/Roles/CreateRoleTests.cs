using System;
using System.Linq;
using NUnit.Framework;
using Moq;
using FluentValidation;
using FluentValidation.Results;
using Weapsy.Domain.Model.Roles;
using Weapsy.Domain.Model.Roles.Commands;
using Weapsy.Domain.Model.Roles.Events;

namespace Weapsy.Domain.Tests.Roles
{
    [TestFixture]
    public class CreateRoleTests
    {
        private CreateRole _command;
        private Mock<IValidator<CreateRole>> _validatorMock;
        private Role _role;
        private RoleCreated _event;

        [SetUp]
        public void Setup()
        {
            _command = new CreateRole
            {
                Id = Guid.NewGuid(),
                Name = "Name"
            };
            _validatorMock = new Mock<IValidator<CreateRole>>();
            _validatorMock.Setup(x => x.Validate(_command)).Returns(new ValidationResult());
            _role = Role.CreateNew(_command, _validatorMock.Object);
            _event = _role.Events.OfType<RoleCreated>().SingleOrDefault();
        }

        [Test]
        public void Should_validate_command()
        {
            _validatorMock.Verify(x => x.Validate(_command));
        }

        [Test]
        public void Should_set_id()
        {
            Assert.AreEqual(_command.Id, _role.Id);
        }

        [Test]
        public void Should_set_role_name()
        {
            Assert.AreEqual(_command.Name, _role.Name);
        }

        [Test]
        public void Should_add_role_created_event()
        {
            Assert.IsNotNull(_event);
        }

        [Test]
        public void Should_set_id_in_role_created_event()
        {
            Assert.AreEqual(_role.Id, _event.AggregateRootId);
        }

        [Test]
        public void Should_set_role_name_in_role_created_event()
        {
            Assert.AreEqual(_role.Name, _event.Name);
        }
    }
}
