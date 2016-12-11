using System;
using System.Linq;
using NUnit.Framework;
using Moq;
using FluentValidation;
using FluentValidation.Results;
using Weapsy.Domain.Apps;
using Weapsy.Domain.Apps.Commands;
using Weapsy.Domain.Apps.Events;
using Weapsy.Tests.Factories;

namespace Weapsy.Domain.Tests.Apps
{
    [TestFixture]
    public class UpdateAppDetailsTests
    {
        private UpdateAppDetails _command;
        private Mock<IValidator<UpdateAppDetails>> _validatorMock;
        private App _app;
        private AppDetailsUpdated _event;

        [SetUp]
        public void Setup()
        {
            _app = AppFactory.App();
            _command = new UpdateAppDetails
            {
                Id = _app.Id,
                Name = "New Name",
                Description = "New Description",
                Folder = "New Folder"
            };
            _validatorMock = new Mock<IValidator<UpdateAppDetails>>();
            _validatorMock.Setup(x => x.Validate(_command)).Returns(new ValidationResult());
            _app.UpdateDetails(_command, _validatorMock.Object);
            _event = _app.Events.OfType<AppDetailsUpdated>().SingleOrDefault();
        }

        [Test]
        public void Should_validate_command()
        {
            _validatorMock.Verify(x => x.Validate(_command));
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
        public void Should_add_app_details_updated_event()
        {
            Assert.IsNotNull(_event);
        }

        [Test]
        public void Should_set_id_in_app_details_updated_event()
        {
            Assert.AreEqual(_app.Id, _event.AggregateRootId);
        }

        [Test]
        public void Should_set_name_in_app_details_updated_event()
        {
            Assert.AreEqual(_app.Name, _event.Name);
        }

        [Test]
        public void Should_set_description_in_app_details_updated_event()
        {
            Assert.AreEqual(_app.Description, _event.Description);
        }

        [Test]
        public void Should_set_folder_in_app_details_updated_event()
        {
            Assert.AreEqual(_app.Folder, _event.Folder);
        }
    }
}
