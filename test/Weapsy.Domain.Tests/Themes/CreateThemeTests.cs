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
    public class CreateThemeTests
    {
        private CreateTheme _command;
        private Mock<IValidator<CreateTheme>> _validatorMock;
        private Mock<IThemeSortOrderGenerator> _sortOrderGeneratorMock;
        private Theme _theme;
        private ThemeCreated _event;

        [SetUp]
        public void Setup()
        {
            _command = new CreateTheme
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Description = "Description",
                Folder = "Folder"
            };            
            _validatorMock = new Mock<IValidator<CreateTheme>>();
            _validatorMock.Setup(x => x.Validate(_command)).Returns(new ValidationResult());
            _sortOrderGeneratorMock = new Mock<IThemeSortOrderGenerator>();
            _sortOrderGeneratorMock.Setup(x => x.GenerateNextSortOrder()).Returns(4);
            _theme = Theme.CreateNew(_command, _validatorMock.Object, _sortOrderGeneratorMock.Object);
            _event = _theme.Events.OfType<ThemeCreated>().SingleOrDefault();
        }

        [Test]
        public void Should_validate_command()
        {
            _validatorMock.Verify(x => x.Validate(_command));
        }

        [Test]
        public void Should_set_id()
        {
            Assert.AreEqual(_command.Id, _theme.Id);
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
        public void Should_call_sort_order_generator()
        {
            _sortOrderGeneratorMock.Verify(x => x.GenerateNextSortOrder());
        }

        [Test]
        public void Should_set_sort_order()
        {
            Assert.AreEqual(4, _theme.SortOrder);
        }

        [Test]
        public void Should_set_status_to_hidden()
        {
            Assert.AreEqual(ThemeStatus.Hidden, _theme.Status);
        }

        [Test]
        public void Should_add_theme_created_event()
        {
            Assert.IsNotNull(_event);
        }

        [Test]
        public void Should_set_id_in_theme_created_event()
        {
            Assert.AreEqual(_theme.Id, _event.AggregateRootId);
        }

        [Test]
        public void Should_set_name_in_theme_created_event()
        {
            Assert.AreEqual(_theme.Name, _event.Name);
        }

        [Test]
        public void Should_set_description_in_theme_created_event()
        {
            Assert.AreEqual(_theme.Description, _event.Description);
        }

        [Test]
        public void Should_set_folder_in_theme_created_event()
        {
            Assert.AreEqual(_theme.Folder, _event.Folder);
        }

        [Test]
        public void Should_set_status_in_theme_created_event()
        {
            Assert.AreEqual(_theme.Status, _event.Status);
        }
    }
}
