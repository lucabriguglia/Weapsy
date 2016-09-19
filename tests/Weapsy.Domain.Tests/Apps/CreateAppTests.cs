using System;
using System.Linq;
using NUnit.Framework;
using Moq;
using FluentValidation;
using FluentValidation.Results;
using Weapsy.Domain.Apps;
using Weapsy.Domain.Apps.Commands;
using Weapsy.Domain.Apps.Events;

namespace Weapsy.Domain.Tests.Apps
{
    [TestFixture]
    public class CreateAppTests
    {
        private CreateApp _command;
        private Mock<IValidator<CreateApp>> _validatorMock;
        private App _app;
        private AppCreated _event;

        [SetUp]
        public void Setup()
        {
            _command = new CreateApp
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Description = "Description",
                Folder = "Folder"
            };            
            _validatorMock = new Mock<IValidator<CreateApp>>();
            _validatorMock.Setup(x => x.Validate(_command)).Returns(new ValidationResult());
            _app = App.CreateNew(_command, _validatorMock.Object);
            _event = _app.Events.OfType<AppCreated>().SingleOrDefault();
        }

        [Test]
        public void Should_validate_command()
        {
            _validatorMock.Verify(x => x.Validate(_command));
        }

        [Test]
        public void Should_set_id()
        {
            Assert.AreEqual(_command.Id, _app.Id);
        }

        [Test]
        public void Should_set_name()
        {
            Assert.AreEqual(_command.Name, _app.Name);
        }

        [Test]
        public void Should_set_description()
        {
            Assert.AreEqual(_command.Description, _app.Description);
        }

        [Test]
        public void Should_set_folder()
        {
            Assert.AreEqual(_command.Folder, _app.Folder);
        }

        [Test]
        public void Should_set_status_to_active()
        {
            Assert.AreEqual(AppStatus.Active, _app.Status);
        }

        [Test]
        public void Should_add_app_created_event()
        {
            Assert.IsNotNull(_event);
        }

        [Test]
        public void Should_set_id_in_app_created_event()
        {
            Assert.AreEqual(_app.Id, _event.AggregateRootId);
        }

        [Test]
        public void Should_set_name_in_app_created_event()
        {
            Assert.AreEqual(_app.Name, _event.Name);
        }

        [Test]
        public void Should_set_description_in_app_created_event()
        {
            Assert.AreEqual(_app.Description, _event.Description);
        }

        [Test]
        public void Should_set_folder_in_app_created_event()
        {
            Assert.AreEqual(_app.Folder, _event.Folder);
        }

        [Test]
        public void Should_set_status_in_app_created_event()
        {
            Assert.AreEqual(_app.Status, _event.Status);
        }
    }
}
