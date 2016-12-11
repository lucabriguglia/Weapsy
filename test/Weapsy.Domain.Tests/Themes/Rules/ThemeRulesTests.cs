using System;
using Moq;
using NUnit.Framework;
using Weapsy.Domain.Themes;
using Weapsy.Domain.Themes.Rules;

namespace Weapsy.Domain.Tests.Themes.Rules
{
    [TestFixture]
    public class ThemeRulesTests
    {
        [Test]
        public void Should_return_false_if_theme_id_is_not_unique()
        {
            var id = Guid.NewGuid();

            var repositoryMock = new Mock<IThemeRepository>();
            repositoryMock.Setup(x => x.GetById(id)).Returns(new Theme());

            var sut = new ThemeRules(repositoryMock.Object);

            var actual = sut.IsThemeIdUnique(id);

            Assert.AreEqual(false, actual);
        }

        [Test]
        public void Should_return_true_if_theme_id_is_unique()
        {
            var id = Guid.NewGuid();

            var repositoryMock = new Mock<IThemeRepository>();
            repositoryMock.Setup(x => x.GetById(id)).Returns((Theme)null);

            var sut = new ThemeRules(repositoryMock.Object);

            var actual = sut.IsThemeIdUnique(id);

            Assert.AreEqual(true, actual);
        }

        [Test]
        public void Should_return_false_if_theme_name_is_not_unique()
        {
            var name = "My Theme";

            var repositoryMock = new Mock<IThemeRepository>();
            repositoryMock.Setup(x => x.GetByName(name)).Returns(new Theme());

            var sut = new ThemeRules(repositoryMock.Object);

            var actual = sut.IsThemeNameUnique(name);

            Assert.AreEqual(false, actual);
        }

        [Test]
        public void Should_return_true_if_theme_name_is_unique()
        {
            var name = "My Theme";

            var repositoryMock = new Mock<IThemeRepository>();
            repositoryMock.Setup(x => x.GetByName(name)).Returns((Theme)null);

            var sut = new ThemeRules(repositoryMock.Object);

            var actual = sut.IsThemeNameUnique(name);

            Assert.AreEqual(true, actual);
        }

        [TestCase("a@b")]
        [TestCase("a!b")]
        [TestCase("a b")]
        [TestCase("")]
        public void Should_return_false_if_theme_folder_is_not_valid(string folder)
        {
            var sut = new ThemeRules(new Mock<IThemeRepository>().Object);
            var actual = sut.IsThemeFolderValid(folder);
            Assert.AreEqual(false, actual);
        }

        [TestCase("ab")]
        [TestCase("a-b")]
        [TestCase("a_b")]
        [TestCase("a1-b2")]
        public void Should_return_true_if_theme_folder_is_valid(string folder)
        {
            var sut = new ThemeRules(new Mock<IThemeRepository>().Object);
            var actual = sut.IsThemeFolderValid(folder);
            Assert.AreEqual(true, actual);
        }

        [Test]
        public void Should_return_false_if_theme_folder_is_not_unique()
        {
            var folder = "my-theme";

            var repositoryMock = new Mock<IThemeRepository>();
            repositoryMock.Setup(x => x.GetByFolder(folder)).Returns(new Theme());

            var sut = new ThemeRules(repositoryMock.Object);

            var actual = sut.IsThemeFolderUnique(folder);

            Assert.AreEqual(false, actual);
        }

        [Test]
        public void Should_return_true_if_theme_folder_is_unique()
        {
            var folder = "my-theme";

            var repositoryMock = new Mock<IThemeRepository>();
            repositoryMock.Setup(x => x.GetByFolder( folder)).Returns((Theme)null);

            var sut = new ThemeRules(repositoryMock.Object);

            var actual = sut.IsThemeFolderUnique(folder);

            Assert.AreEqual(true, actual);
        }
    }
}
