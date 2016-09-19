using System;
using System.Linq;
using NUnit.Framework;
using Moq;
using FluentValidation;
using FluentValidation.Results;
using Weapsy.Domain.ModuleTypes;
using Weapsy.Domain.ModuleTypes.Commands;
using Weapsy.Domain.ModuleTypes.Events;

namespace Weapsy.Domain.Tests.ModuleTypes
{
    [TestFixture]
    public class CreateModuleTypeTests
    {
        private CreateModuleType _command;
        private Mock<IValidator<CreateModuleType>> _validatorMock;
        private ModuleType _moduleType;
        private ModuleTypeCreated _event;

        [SetUp]
        public void Setup()
        {
            _command = new CreateModuleType
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
            _validatorMock = new Mock<IValidator<CreateModuleType>>();
            _validatorMock.Setup(x => x.Validate(_command)).Returns(new ValidationResult());
            _moduleType = ModuleType.CreateNew(_command, _validatorMock.Object);
            _event = _moduleType.Events.OfType<ModuleTypeCreated>().SingleOrDefault();
        }

        [Test]
        public void Should_validate_command()
        {
            _validatorMock.Verify(x => x.Validate(_command));
        }

        [Test]
        public void Should_set_id()
        {
            Assert.AreEqual(_command.Id, _moduleType.Id);
        }

        [Test]
        public void Should_set_app_id()
        {
            Assert.AreEqual(_command.AppId, _moduleType.AppId);
        }

        [Test]
        public void Should_set_name()
        {
            Assert.AreEqual(_command.Name, _moduleType.Name);
        }

        [Test]
        public void Should_set_title()
        {
            Assert.AreEqual(_command.Title, _moduleType.Title);
        }

        [Test]
        public void Should_set_description()
        {
            Assert.AreEqual(_command.Description, _moduleType.Description);
        }

        [Test]
        public void Should_set_view_type()
        {
            Assert.AreEqual(_command.ViewType, _moduleType.ViewType);
        }

        [Test]
        public void Should_set_view_name()
        {
            Assert.AreEqual(_command.ViewName, _moduleType.ViewName);
        }

        [Test]
        public void Should_set_edit_type()
        {
            Assert.AreEqual(_command.EditType, _moduleType.EditType);
        }

        [Test]
        public void Should_set_edit_url()
        {
            Assert.AreEqual(_command.EditUrl, _moduleType.EditUrl);
        }

        [Test]
        public void Should_set_status_to_active()
        {
            Assert.AreEqual(ModuleTypeStatus.Active, _moduleType.Status);
        }

        [Test]
        public void Should_add_module_type_created_event()
        {
            Assert.IsNotNull(_event);
        }

        [Test]
        public void Should_set_id_in_module_type_created_event()
        {
            Assert.AreEqual(_moduleType.Id, _event.AggregateRootId);
        }

        [Test]
        public void Should_set_app_id_in_module_type_created_event()
        {
            Assert.AreEqual(_moduleType.AppId, _event.AppId);
        }

        [Test]
        public void Should_set_name_in_module_type_created_event()
        {
            Assert.AreEqual(_moduleType.Name, _event.Name);
        }

        [Test]
        public void Should_set_title_in_module_type_created_event()
        {
            Assert.AreEqual(_moduleType.Title, _event.Title);
        }

        [Test]
        public void Should_set_description_in_module_type_created_event()
        {
            Assert.AreEqual(_moduleType.Description, _event.Description);
        }

        [Test]
        public void Should_set_view_type_in_module_type_created_event()
        {
            Assert.AreEqual(_moduleType.ViewType, _event.ViewType);
        }

        [Test]
        public void Should_set_view_name_in_module_type_created_event()
        {
            Assert.AreEqual(_moduleType.ViewName, _event.ViewName);
        }

        [Test]
        public void Should_set_edit_type_in_module_type_created_event()
        {
            Assert.AreEqual(_moduleType.EditType, _event.EditType);
        }

        [Test]
        public void Should_set_edit_url_in_module_type_created_event()
        {
            Assert.AreEqual(_moduleType.EditUrl, _event.EditUrl);
        }

        [Test]
        public void Should_set_status_in_module_type_created_event()
        {
            Assert.AreEqual(_moduleType.Status, _event.Status);
        }
    }
}
