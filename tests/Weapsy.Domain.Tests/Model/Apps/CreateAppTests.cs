using System;
using System.Linq;
using NUnit.Framework;
using Moq;
using FluentValidation;
using FluentValidation.Results;
using Weapsy.Domain.Model.Apps;
using Weapsy.Domain.Model.Apps.Commands;
using Weapsy.Domain.Model.Apps.Events;

namespace Weapsy.Domain.Tests.Apps
{
    [TestFixture]
    public class CreateAppTests
    {
        private CreateApp command;
        private Mock<IValidator<CreateApp>> validatorMock;
        private App app;
        private AppCreated @event;

        [SetUp]
        public void Setup()
        {
            command = new CreateApp
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Description = "Description",
                Folder = "Folder"
            };            
            validatorMock = new Mock<IValidator<CreateApp>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());
            app = App.CreateNew(command, validatorMock.Object);
            @event = app.Events.OfType<AppCreated>().SingleOrDefault();
        }

        [Test]
        public void Should_validate_command()
        {
            validatorMock.Verify(x => x.Validate(command));
        }

        [Test]
        public void Should_set_id()
        {
            Assert.AreEqual(command.Id, app.Id);
        }

        [Test]
        public void Should_set_name()
        {
            Assert.AreEqual(command.Name, app.Name);
        }

        [Test]
        public void Should_set_description()
        {
            Assert.AreEqual(command.Description, app.Description);
        }

        [Test]
        public void Should_set_folder()
        {
            Assert.AreEqual(command.Folder, app.Folder);
        }

        [Test]
        public void Should_set_status_to_active()
        {
            Assert.AreEqual(AppStatus.Active, app.Status);
        }

        [Test]
        public void Should_add_app_created_event()
        {
            Assert.IsNotNull(@event);
        }

        [Test]
        public void Should_set_id_in_app_created_event()
        {
            Assert.AreEqual(app.Id, @event.AggregateRootId);
        }

        [Test]
        public void Should_set_name_in_app_created_event()
        {
            Assert.AreEqual(app.Name, @event.Name);
        }

        [Test]
        public void Should_set_description_in_app_created_event()
        {
            Assert.AreEqual(app.Description, @event.Description);
        }

        [Test]
        public void Should_set_folder_in_app_created_event()
        {
            Assert.AreEqual(app.Folder, @event.Folder);
        }

        [Test]
        public void Should_set_status_in_app_created_event()
        {
            Assert.AreEqual(app.Status, @event.Status);
        }
    }
}
