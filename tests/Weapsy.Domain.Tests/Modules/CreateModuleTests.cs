using System;
using System.Linq;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using NUnit.Framework;
using Weapsy.Domain.Modules;
using Weapsy.Domain.Modules.Commands;
using Weapsy.Domain.Modules.Events;

namespace Weapsy.Domain.Tests.Modules
{
    [TestFixture]
    public class CreateModuleTests
    {
        private CreateModule _command;
        private Mock<IValidator<CreateModule>> _validatorMock;
        private Module _module;
        private ModuleCreated _event;

        [SetUp]
        public void Setup()
        {
            _command = new CreateModule
            {
                SiteId = Guid.NewGuid(),
                ModuleTypeId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Title = "Title"
            };
            _validatorMock = new Mock<IValidator<CreateModule>>();
            _validatorMock.Setup(x => x.Validate(_command)).Returns(new ValidationResult());
            _module = Module.CreateNew(_command, _validatorMock.Object);
            _event = _module.Events.OfType<ModuleCreated>().SingleOrDefault();
        }

        [Test]
        public void Should_validate_command()
        {
            _validatorMock.Verify(x => x.Validate(_command));
        }

        [Test]
        public void Should_set_id()
        {
            Assert.AreEqual(_command.Id, _module.Id);
        }

        [Test]
        public void Should_set_site_id()
        {
            Assert.AreEqual(_command.SiteId, _module.SiteId);
        }

        [Test]
        public void Should_set_module_type_id()
        {
            Assert.AreEqual(_command.ModuleTypeId, _module.ModuleTypeId);
        }

        [Test]
        public void Should_set_title()
        {
            Assert.AreEqual(_command.Title, _module.Title);
        }

        [Test]
        public void Should_set_status_to_active()
        {
            Assert.AreEqual(ModuleStatus.Active, _module.Status);
        }

        [Test]
        public void Should_add_module_created_event()
        {
            Assert.IsNotNull(_event);
        }

        [Test]
        public void Should_set_id_in_module_created_event()
        {
            Assert.AreEqual(_module.Id, _event.AggregateRootId);
        }

        [Test]
        public void Should_set_site_id_in_module_created_event()
        {
            Assert.AreEqual(_module.SiteId, _event.SiteId);
        }

        [Test]
        public void Should_set_module_type_id_in_module_created_event()
        {
            Assert.AreEqual(_module.ModuleTypeId, _event.ModuleTypeId);
        }

        [Test]
        public void Should_set_title_in_module_created_event()
        {
            Assert.AreEqual(_module.Title, _event.Title);
        }

        [Test]
        public void Should_set_module_status_in_module_created_event()
        {
            Assert.AreEqual(_module.Status, _event.Status);
        }
    }
}
