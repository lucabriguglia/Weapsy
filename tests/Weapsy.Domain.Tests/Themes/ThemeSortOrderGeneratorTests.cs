using System;
using NUnit.Framework;
using Moq;
using Weapsy.Domain.Themes;

namespace Weapsy.Domain.Tests.Themes
{
    [TestFixture]
    public class ThemeSortOrderGeneratorTests
    {
        [Test]
        public void Should_add_one_to_active_themes_count_when_generate_new_sort_order()
        {
            var siteId = Guid.NewGuid();

            var themeRepositoryMock = new Mock<IThemeRepository>();
            themeRepositoryMock.Setup(x => x.GetThemesCount()).Returns(4);

            var sut = new ThemeSortOrderGenerator(themeRepositoryMock.Object);

            var actual = sut.GenerateNextSortOrder();

            Assert.AreEqual(5, actual);
        }
    }
}
