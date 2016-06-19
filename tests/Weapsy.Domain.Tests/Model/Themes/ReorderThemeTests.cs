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
    public class ReorderThemeTests
    {
        private Theme theme;
        private Guid themeId;
        private int newSortOrder;
        private ThemeReordered @event;

        [SetUp]
        public void Setup()
        {
            themeId = Guid.NewGuid();
            newSortOrder = 1;

            var command = new CreateTheme
            {
                Id = themeId,
                Name = "Name",
                Description = "Description",
                Folder = "Folder"
            };

            var validatorMock = new Mock<IValidator<CreateTheme>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            var sortOrderGeneratorMock = new Mock<IThemeSortOrderGenerator>();
            sortOrderGeneratorMock.Setup(x => x.GenerateNextSortOrder()).Returns(2);

            theme = Theme.CreateNew(command, validatorMock.Object, sortOrderGeneratorMock.Object);
            
            theme.Reorder(newSortOrder);

            @event = theme.Events.OfType<ThemeReordered>().SingleOrDefault();
        }

        [Test]
        public void Should_set_sort_order()
        {
            Assert.AreEqual(newSortOrder, theme.SortOrder);
        }

        [Test]
        public void Should_add_theme_reordered_event()
        {
            Assert.IsNotNull(@event);
        }

        [Test]
        public void Should_set_id_in_theme_reordered_event()
        {
            Assert.AreEqual(theme.Id, @event.AggregateRootId);
        }

        [Test]
        public void Should_set_sort_order_in_theme_reordered_event()
        {
            Assert.AreEqual(theme.SortOrder, @event.SortOrder);
        }

        [Test]
        public void Should_throw_exception_if_theme_is_deleted()
        {
            theme.Delete();
            Assert.Throws<Exception>(() => theme.Reorder(1));
        }
    }
}
