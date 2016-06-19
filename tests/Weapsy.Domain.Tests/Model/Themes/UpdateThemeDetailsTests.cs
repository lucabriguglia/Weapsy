using System;
using System.Linq;
using NUnit.Framework;
using Moq;
using FluentValidation;
using FluentValidation.Results;
using Weapsy.Domain.Model.Themes;
using Weapsy.Domain.Model.Themes.Commands;
using Weapsy.Domain.Model.Themes.Events;

namespace Weapsy.Domain.Tests.Themes
{
    [TestFixture]
    public class UpdateThemeDetailsTests
    {
        private UpdateThemeDetails command;
        private Mock<IValidator<UpdateThemeDetails>> validatorMock;
        private Theme theme;
        private ThemeDetailsUpdated @event;

        [SetUp]
        public void Setup()
        {
            command = new UpdateThemeDetails
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Description = "Description",
                Folder = "folder"
            };            
            validatorMock = new Mock<IValidator<UpdateThemeDetails>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());
            theme = new Theme();
            theme.UpdateDetails(command, validatorMock.Object);
            @event = theme.Events.OfType<ThemeDetailsUpdated>().SingleOrDefault();
        }

        [Test]
        public void Should_validate_command()
        {
            validatorMock.Verify(x => x.Validate(command));
        }

        [Test]
        public void Should_set_name()
        {
            Assert.AreEqual(command.Name, theme.Name);
        }

        [Test]
        public void Should_set_description()
        {
            Assert.AreEqual(command.Description, theme.Description);
        }

        [Test]
        public void Should_set_folder()
        {
            Assert.AreEqual(command.Folder, theme.Folder);
        }

        [Test]
        public void Should_add_theme_details_updated_event()
        {
            Assert.IsNotNull(@event);
        }

        [Test]
        public void Should_set_id_in_theme_details_updated_event()
        {
            Assert.AreEqual(theme.Id, @event.AggregateRootId);
        }

        [Test]
        public void Should_set_name_in_theme_details_updated_event()
        {
            Assert.AreEqual(theme.Name, @event.Name);
        }

        [Test]
        public void Should_set_description_in_theme_details_updated_event()
        {
            Assert.AreEqual(theme.Description, @event.Description);
        }

        [Test]
        public void Should_set_folder_in_theme_details_updated_event()
        {
            Assert.AreEqual(theme.Folder, @event.Folder);
        }
    }
}
