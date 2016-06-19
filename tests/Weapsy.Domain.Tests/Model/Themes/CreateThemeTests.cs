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
    public class CreateThemeTests
    {
        private CreateTheme command;
        private Mock<IValidator<CreateTheme>> validatorMock;
        private Mock<IThemeSortOrderGenerator> sortOrderGeneratorMock;
        private Theme theme;
        private ThemeCreated @event;

        [SetUp]
        public void Setup()
        {
            command = new CreateTheme
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Description = "Description",
                Folder = "Folder"
            };            
            validatorMock = new Mock<IValidator<CreateTheme>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());
            sortOrderGeneratorMock = new Mock<IThemeSortOrderGenerator>();
            sortOrderGeneratorMock.Setup(x => x.GenerateNextSortOrder()).Returns(4);
            theme = Theme.CreateNew(command, validatorMock.Object, sortOrderGeneratorMock.Object);
            @event = theme.Events.OfType<ThemeCreated>().SingleOrDefault();
        }

        [Test]
        public void Should_validate_command()
        {
            validatorMock.Verify(x => x.Validate(command));
        }

        [Test]
        public void Should_set_id()
        {
            Assert.AreEqual(command.Id, theme.Id);
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
        public void Should_call_sort_order_generator()
        {
            sortOrderGeneratorMock.Verify(x => x.GenerateNextSortOrder());
        }

        [Test]
        public void Should_set_sort_order()
        {
            Assert.AreEqual(4, theme.SortOrder);
        }

        [Test]
        public void Should_set_status_to_hidden()
        {
            Assert.AreEqual(ThemeStatus.Hidden, theme.Status);
        }

        [Test]
        public void Should_add_theme_created_event()
        {
            Assert.IsNotNull(@event);
        }

        [Test]
        public void Should_set_id_in_theme_created_event()
        {
            Assert.AreEqual(theme.Id, @event.AggregateRootId);
        }

        [Test]
        public void Should_set_name_in_theme_created_event()
        {
            Assert.AreEqual(theme.Name, @event.Name);
        }

        [Test]
        public void Should_set_description_in_theme_created_event()
        {
            Assert.AreEqual(theme.Description, @event.Description);
        }

        [Test]
        public void Should_set_folder_in_theme_created_event()
        {
            Assert.AreEqual(theme.Folder, @event.Folder);
        }

        [Test]
        public void Should_set_status_in_theme_created_event()
        {
            Assert.AreEqual(theme.Status, @event.Status);
        }
    }
}
