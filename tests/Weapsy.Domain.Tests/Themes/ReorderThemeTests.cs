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
    public class ReorderThemeTests
    {
        private Theme _theme;
        private Guid _themeId;
        private int _newSortOrder;
        private ThemeReordered _event;

        [SetUp]
        public void Setup()
        {
            _themeId = Guid.NewGuid();
            _newSortOrder = 1;

            var command = new CreateTheme
            {
                Id = _themeId,
                Name = "Name",
                Description = "Description",
                Folder = "Folder"
            };

            var validatorMock = new Mock<IValidator<CreateTheme>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            var sortOrderGeneratorMock = new Mock<IThemeSortOrderGenerator>();
            sortOrderGeneratorMock.Setup(x => x.GenerateNextSortOrder()).Returns(2);

            _theme = Theme.CreateNew(command, validatorMock.Object, sortOrderGeneratorMock.Object);
            
            _theme.Reorder(_newSortOrder);

            _event = _theme.Events.OfType<ThemeReordered>().SingleOrDefault();
        }

        [Test]
        public void Should_set_sort_order()
        {
            Assert.AreEqual(_newSortOrder, _theme.SortOrder);
        }

        [Test]
        public void Should_add_theme_reordered_event()
        {
            Assert.IsNotNull(_event);
        }

        [Test]
        public void Should_set_id_in_theme_reordered_event()
        {
            Assert.AreEqual(_theme.Id, _event.AggregateRootId);
        }

        [Test]
        public void Should_set_sort_order_in_theme_reordered_event()
        {
            Assert.AreEqual(_theme.SortOrder, _event.SortOrder);
        }

        [Test]
        public void Should_throw_exception_if_theme_is_deleted()
        {
            _theme.Delete();
            Assert.Throws<Exception>(() => _theme.Reorder(1));
        }
    }
}
