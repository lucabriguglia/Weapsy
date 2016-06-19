using System;
using System.Linq;
using NUnit.Framework;
using Moq;
using FluentValidation;
using FluentValidation.Results;
using Weapsy.Domain.Model.ModuleTypes;
using Weapsy.Domain.Model.ModuleTypes.Commands;
using Weapsy.Domain.Model.ModuleTypes.Events;

namespace Weapsy.Domain.Tests.ModuleTypes
{
    [TestFixture]
    public class CreateModuleTypeTests
    {
        private CreateModuleType command;
        private Mock<IValidator<CreateModuleType>> validatorMock;
        private ModuleType moduleType;
        private ModuleTypeCreated @event;

        [SetUp]
        public void Setup()
        {
            command = new CreateModuleType
            {
                AppId = Guid.Empty,
                Id = Guid.NewGuid(),
                Name = "Name",
                Title = "Title",
                Description = "Description",
                ViewType = ViewType.View,
                ViewName = "ViewName",
                EditType = EditType.Page,
                EditUrl = "EditUrl"
            };            
            validatorMock = new Mock<IValidator<CreateModuleType>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());
            moduleType = ModuleType.CreateNew(command, validatorMock.Object);
            @event = moduleType.Events.OfType<ModuleTypeCreated>().SingleOrDefault();
        }

        [Test]
        public void Should_validate_command()
        {
            validatorMock.Verify(x => x.Validate(command));
        }

        [Test]
        public void Should_set_id()
        {
            Assert.AreEqual(command.Id, moduleType.Id);
        }

        [Test]
        public void Should_set_app_id()
        {
            Assert.AreEqual(command.AppId, moduleType.AppId);
        }

        [Test]
        public void Should_set_name()
        {
            Assert.AreEqual(command.Name, moduleType.Name);
        }

        [Test]
        public void Should_set_title()
        {
            Assert.AreEqual(command.Title, moduleType.Title);
        }

        [Test]
        public void Should_set_description()
        {
            Assert.AreEqual(command.Description, moduleType.Description);
        }

        [Test]
        public void Should_set_view_type()
        {
            Assert.AreEqual(command.ViewType, moduleType.ViewType);
        }

        [Test]
        public void Should_set_view_name()
        {
            Assert.AreEqual(command.ViewName, moduleType.ViewName);
        }

        [Test]
        public void Should_set_edit_type()
        {
            Assert.AreEqual(command.EditType, moduleType.EditType);
        }

        [Test]
        public void Should_set_edit_url()
        {
            Assert.AreEqual(command.EditUrl, moduleType.EditUrl);
        }

        [Test]
        public void Should_set_status_to_active()
        {
            Assert.AreEqual(ModuleTypeStatus.Active, moduleType.Status);
        }

        [Test]
        public void Should_add_module_type_created_event()
        {
            Assert.IsNotNull(@event);
        }

        [Test]
        public void Should_set_id_in_module_type_created_event()
        {
            Assert.AreEqual(moduleType.Id, @event.AggregateRootId);
        }

        [Test]
        public void Should_set_app_id_in_module_type_created_event()
        {
            Assert.AreEqual(moduleType.AppId, @event.AppId);
        }

        [Test]
        public void Should_set_name_in_module_type_created_event()
        {
            Assert.AreEqual(moduleType.Name, @event.Name);
        }

        [Test]
        public void Should_set_title_in_module_type_created_event()
        {
            Assert.AreEqual(moduleType.Title, @event.Title);
        }

        [Test]
        public void Should_set_description_in_module_type_created_event()
        {
            Assert.AreEqual(moduleType.Description, @event.Description);
        }

        [Test]
        public void Should_set_view_type_in_module_type_created_event()
        {
            Assert.AreEqual(moduleType.ViewType, @event.ViewType);
        }

        [Test]
        public void Should_set_view_name_in_module_type_created_event()
        {
            Assert.AreEqual(moduleType.ViewName, @event.ViewName);
        }

        [Test]
        public void Should_set_edit_type_in_module_type_created_event()
        {
            Assert.AreEqual(moduleType.EditType, @event.EditType);
        }

        [Test]
        public void Should_set_edit_url_in_module_type_created_event()
        {
            Assert.AreEqual(moduleType.EditUrl, @event.EditUrl);
        }

        [Test]
        public void Should_set_status_in_module_type_created_event()
        {
            Assert.AreEqual(moduleType.Status, @event.Status);
        }
    }
}
