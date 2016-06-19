using System;
using System.Linq;
using NUnit.Framework;
using Moq;
using FluentValidation;
using FluentValidation.Results;
using Weapsy.Domain.Model.Apps;
using Weapsy.Domain.Model.Apps.Commands;
using Weapsy.Domain.Model.Apps.Events;
using Weapsy.Tests.Factories;

namespace Weapsy.Domain.Tests.Apps
{
    [TestFixture]
    public class UpdateAppDetailsTests
    {
        private UpdateAppDetails command;
        private Mock<IValidator<UpdateAppDetails>> validatorMock;
        private App app;
        private AppDetailsUpdated @event;

        [SetUp]
        public void Setup()
        {
            app = AppFactory.App();
            command = new UpdateAppDetails
            {
                Id = app.Id,
                Name = "New Name",
                Description = "New Description",
                Folder = "New Folder"
            };
            validatorMock = new Mock<IValidator<UpdateAppDetails>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());
            app.UpdateDetails(command, validatorMock.Object);
            @event = app.Events.OfType<AppDetailsUpdated>().SingleOrDefault();
        }

        [Test]
        public void Should_validate_command()
        {
            validatorMock.Verify(x => x.Validate(command));
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
        public void Should_add_app_details_updated_event()
        {
            Assert.IsNotNull(@event);
        }

        [Test]
        public void Should_set_id_in_app_details_updated_event()
        {
            Assert.AreEqual(app.Id, @event.AggregateRootId);
        }

        [Test]
        public void Should_set_name_in_app_details_updated_event()
        {
            Assert.AreEqual(app.Name, @event.Name);
        }

        [Test]
        public void Should_set_description_in_app_details_updated_event()
        {
            Assert.AreEqual(app.Description, @event.Description);
        }

        [Test]
        public void Should_set_folder_in_app_details_updated_event()
        {
            Assert.AreEqual(app.Folder, @event.Folder);
        }
    }
}
