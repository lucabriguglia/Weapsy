using System;
using System.Linq;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using NUnit.Framework;
using Weapsy.Domain.Model.ModuleTypes;
using Weapsy.Domain.Model.ModuleTypes.Commands;
using Weapsy.Domain.Model.ModuleTypes.Events;

namespace Weapsy.Domain.Tests.ModuleTypes
{
    [TestFixture]
    public class UpdateModuleTypeDetailsTests
    {
        private UpdateModuleTypeDetails command;
        private Mock<IValidator<UpdateModuleTypeDetails>> validatorMock;
        private ModuleType moduleType;
        private ModuleTypeDetailsUpdated @event;

        [SetUp]
        public void Setup()
        {
            command = new UpdateModuleTypeDetails
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Title = "Title",
                Description = "Description"
            };
            validatorMock = new Mock<IValidator<UpdateModuleTypeDetails>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());
            moduleType = new ModuleType();
            moduleType.UpdateDetails(command, validatorMock.Object);
            @event = moduleType.Events.OfType<ModuleTypeDetailsUpdated>().SingleOrDefault();
        }

        [Test]
        public void Should_validate_command()
        {
            validatorMock.Verify(x => x.Validate(command));
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
        public void Should_add_module_type_details_updated_event()
        {
            Assert.IsNotNull(@event);
        }

        [Test]
        public void Should_set_id_in_module_type_details_updated_event()
        {
            Assert.AreEqual(moduleType.Id, @event.AggregateRootId);
        }

        [Test]
        public void Should_set_name_in_module_type_details_updated_event()
        {
            Assert.AreEqual(moduleType.Name, @event.Name);
        }

        [Test]
        public void Should_set_head_title_in_module_type_details_updated_event()
        {
            Assert.AreEqual(moduleType.Title, @event.Title);
        }

        [Test]
        public void Should_set_meta_description_in_module_type_details_updated_event()
        {
            Assert.AreEqual(moduleType.Description, @event.Description);
        }

        [Test]
        public void Should_set_view_type_in_module_type_details_updated_event()
        {
            Assert.AreEqual(moduleType.ViewType, @event.ViewType);
        }

        [Test]
        public void Should_set_view_name_in_module_type_details_updated_event()
        {
            Assert.AreEqual(moduleType.ViewName, @event.ViewName);
        }

        [Test]
        public void Should_set_edit_type_in_module_type_details_updated_event()
        {
            Assert.AreEqual(moduleType.EditType, @event.EditType);
        }

        [Test]
        public void Should_set_edit_url_in_module_type_details_updated_event()
        {
            Assert.AreEqual(moduleType.EditUrl, @event.EditUrl);
        }
    }
}
