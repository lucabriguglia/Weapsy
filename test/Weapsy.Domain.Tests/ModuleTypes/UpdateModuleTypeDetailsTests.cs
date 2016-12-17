using System;
using System.Linq;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using NUnit.Framework;
using Weapsy.Domain.ModuleTypes;
using Weapsy.Domain.ModuleTypes.Commands;
using Weapsy.Domain.ModuleTypes.Events;

namespace Weapsy.Domain.Tests.ModuleTypes
{
    [TestFixture]
    public class UpdateModuleTypeDetailsTests
    {
        private UpdateModuleTypeDetails _command;
        private Mock<IValidator<UpdateModuleTypeDetails>> _validatorMock;
        private ModuleType _moduleType;
        private ModuleTypeDetailsUpdated _event;

        [SetUp]
        public void Setup()
        {
            _command = new UpdateModuleTypeDetails
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Title = "Title",
                Description = "Description"
            };
            _validatorMock = new Mock<IValidator<UpdateModuleTypeDetails>>();
            _validatorMock.Setup(x => x.Validate(_command)).Returns(new ValidationResult());
            _moduleType = new ModuleType();
            _moduleType.UpdateDetails(_command, _validatorMock.Object);
            _event = _moduleType.Events.OfType<ModuleTypeDetailsUpdated>().SingleOrDefault();
        }

        [Test]
        public void Should_validate_command()
        {
            _validatorMock.Verify(x => x.Validate(_command));
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
        public void Should_add_module_type_details_updated_event()
        {
            Assert.IsNotNull(_event);
        }

        [Test]
        public void Should_set_id_in_module_type_details_updated_event()
        {
            Assert.AreEqual(_moduleType.Id, _event.AggregateRootId);
        }

        [Test]
        public void Should_set_name_in_module_type_details_updated_event()
        {
            Assert.AreEqual(_moduleType.Name, _event.Name);
        }

        [Test]
        public void Should_set_head_title_in_module_type_details_updated_event()
        {
            Assert.AreEqual(_moduleType.Title, _event.Title);
        }

        [Test]
        public void Should_set_meta_description_in_module_type_details_updated_event()
        {
            Assert.AreEqual(_moduleType.Description, _event.Description);
        }

        [Test]
        public void Should_set_view_type_in_module_type_details_updated_event()
        {
            Assert.AreEqual(_moduleType.ViewType, _event.ViewType);
        }

        [Test]
        public void Should_set_view_name_in_module_type_details_updated_event()
        {
            Assert.AreEqual(_moduleType.ViewName, _event.ViewName);
        }

        [Test]
        public void Should_set_edit_type_in_module_type_details_updated_event()
        {
            Assert.AreEqual(_moduleType.EditType, _event.EditType);
        }

        [Test]
        public void Should_set_edit_url_in_module_type_details_updated_event()
        {
            Assert.AreEqual(_moduleType.EditUrl, _event.EditUrl);
        }
    }
}
