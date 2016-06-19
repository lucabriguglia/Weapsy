using System;
using System.Linq;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using NUnit.Framework;
using Weapsy.Domain.Model.Modules;
using Weapsy.Domain.Model.Modules.Commands;
using Weapsy.Domain.Model.Modules.Events;

namespace Weapsy.Domain.Tests.Modules
{
    [TestFixture]
    public class CreateModuleTests
    {
        private CreateModule command;
        private Mock<IValidator<CreateModule>> validatorMock;
        private Module module;
        private ModuleCreated @event;

        [SetUp]
        public void Setup()
        {
            command = new CreateModule
            {
                SiteId = Guid.NewGuid(),
                ModuleTypeId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Title = "Title"
            };
            validatorMock = new Mock<IValidator<CreateModule>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());
            module = Module.CreateNew(command, validatorMock.Object);
            @event = module.Events.OfType<ModuleCreated>().SingleOrDefault();
        }

        [Test]
        public void Should_validate_command()
        {
            validatorMock.Verify(x => x.Validate(command));
        }

        [Test]
        public void Should_set_id()
        {
            Assert.AreEqual(command.Id, module.Id);
        }

        [Test]
        public void Should_set_site_id()
        {
            Assert.AreEqual(command.SiteId, module.SiteId);
        }

        [Test]
        public void Should_set_module_type_id()
        {
            Assert.AreEqual(command.ModuleTypeId, module.ModuleTypeId);
        }

        [Test]
        public void Should_set_title()
        {
            Assert.AreEqual(command.Title, module.Title);
        }

        [Test]
        public void Should_set_status_to_active()
        {
            Assert.AreEqual(ModuleStatus.Active, module.Status);
        }

        [Test]
        public void Should_add_module_created_event()
        {
            Assert.IsNotNull(@event);
        }

        [Test]
        public void Should_set_id_in_module_created_event()
        {
            Assert.AreEqual(module.Id, @event.AggregateRootId);
        }

        [Test]
        public void Should_set_site_id_in_module_created_event()
        {
            Assert.AreEqual(module.SiteId, @event.SiteId);
        }

        [Test]
        public void Should_set_module_type_id_in_module_created_event()
        {
            Assert.AreEqual(module.ModuleTypeId, @event.ModuleTypeId);
        }

        [Test]
        public void Should_set_title_in_module_created_event()
        {
            Assert.AreEqual(module.Title, @event.Title);
        }

        [Test]
        public void Should_set_module_status_in_module_created_event()
        {
            Assert.AreEqual(module.Status, @event.Status);
        }
    }
}
