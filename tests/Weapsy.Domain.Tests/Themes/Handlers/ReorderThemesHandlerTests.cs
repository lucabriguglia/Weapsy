using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Weapsy.Domain.Themes;
using Weapsy.Domain.Themes.Commands;
using Weapsy.Domain.Themes.Handlers;
using System.Reflection;

namespace Weapsy.Domain.Tests.Themes.Handlers
{
    [TestFixture]
    public class ReorderThemesHandlerTests
    {
        [Test]
        public void Should_change_sort_order_and_update_themes()
        {
            var theme1Id = Guid.NewGuid();
            var theme2Id = Guid.NewGuid();

            var command = new ReorderThemes
            {
                Themes = new List<Guid>
                {
                    theme1Id,
                    theme2Id
                }
            };

            var theme1 = new Theme();
            typeof(Theme).GetTypeInfo().GetProperty("Id").SetValue(theme1, theme1Id, null);
            typeof(Theme).GetTypeInfo().GetProperty("SortOrder").SetValue(theme1, 2, null);

            var theme2 = new Theme();
            typeof(Theme).GetTypeInfo().GetProperty("Id").SetValue(theme2, theme2Id, null);
            typeof(Theme).GetTypeInfo().GetProperty("SortOrder").SetValue(theme2, 1, null);

            var themeRepositoryMock = new Mock<IThemeRepository>();
            themeRepositoryMock.Setup(x => x.GetById(theme1Id)).Returns(theme1);
            themeRepositoryMock.Setup(x => x.GetById(theme2Id)).Returns(theme2);
            themeRepositoryMock.Setup(x => x.Update(It.IsAny<List<Theme>>()));

            var reorderThemesHandler = new ReorderThemesHandler(themeRepositoryMock.Object);
            reorderThemesHandler.Handle(command);

            Assert.AreEqual(1, theme1.SortOrder);
            Assert.AreEqual(2, theme2.SortOrder);
            themeRepositoryMock.Verify(x => x.Update(It.IsAny<List<Theme>>()));
        }

        [Test]
        public void Should_throw_exception_if_theme_not_found()
        {
            var themeId = Guid.NewGuid();

            var command = new ReorderThemes
            {
                Themes = new List<Guid>
                {
                    themeId
                }
            };

            var themeRepositoryMock = new Mock<IThemeRepository>();
            themeRepositoryMock.Setup(x => x.GetById(themeId)).Returns((Theme)null);

            var reorderThemesHandler = new ReorderThemesHandler(themeRepositoryMock.Object);

            Assert.Throws<Exception>(() => reorderThemesHandler.Handle(command));
        }
    }
}
