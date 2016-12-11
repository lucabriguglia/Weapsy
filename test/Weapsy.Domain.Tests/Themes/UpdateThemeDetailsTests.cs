using System;
using System.Linq;
using NUnit.Framework;
using Moq;
using FluentValidation;
using FluentValidation.Results;
using Weapsy.Domain.Themes;
using Weapsy.Domain.Themes.Commands;
using Weapsy.Domain.Themes.Events;

namespace Weapsy.Domain.Tests.Themes
{
    [TestFixture]
    public class UpdateThemeDetailsTests
    {
        private UpdateThemeDetails _command;
        private Mock<IValidator<UpdateThemeDetails>> _validatorMock;
        private Theme _theme;
        private ThemeDetailsUpdated _event;

        [SetUp]
        public void Setup()
        {
            _command = new UpdateThemeDetails
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Description = "Description",
                Folder = "folder"
            };            
            _validatorMock = new Mock<IValidator<UpdateThemeDetails>>();
            _validatorMock.Setup(x => x.Validate(_command)).Returns(new ValidationResult());
            _theme = new Theme();
            _theme.UpdateDetails(_command, _validatorMock.Object);
            _event = _theme.Events.OfType<ThemeDetailsUpdated>().SingleOrDefault();
        }

        [Test]
        public void Should_validate_command()
        {
            _validatorMock.Verify(x => x.Validate(_command));
        }

        [Test]
        public void Should_set_name()
        {
            Assert.AreEqual(_command.Name, _theme.Name);
        }

        [Test]
        public void Should_set_description()
        {
            Assert.AreEqual(_command.Description, _theme.Description);
        }

        [Test]
        public void Should_set_folder()
        {
            Assert.AreEqual(_command.Folder, _theme.Folder);
        }

        [Test]
        public void Should_add_theme_details_updated_event()
        {
            Assert.IsNotNull(_event);
        }

        [Test]
        public void Should_set_id_in_theme_details_updated_event()
        {
            Assert.AreEqual(_theme.Id, _event.AggregateRootId);
        }

        [Test]
        public void Should_set_name_in_theme_details_updated_event()
        {
            Assert.AreEqual(_theme.Name, _event.Name);
        }

        [Test]
        public void Should_set_description_in_theme_details_updated_event()
        {
            Assert.AreEqual(_theme.Description, _event.Description);
        }

        [Test]
        public void Should_set_folder_in_theme_details_updated_event()
        {
            Assert.AreEqual(_theme.Folder, _event.Folder);
        }
    }
}
