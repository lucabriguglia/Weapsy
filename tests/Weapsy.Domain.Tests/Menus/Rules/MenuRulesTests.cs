using System;
using Moq;
using NUnit.Framework;
using Weapsy.Domain.Menus;
using Weapsy.Domain.Menus.Rules;

namespace Weapsy.Domain.Tests.Menus.Rules
{
    [TestFixture]
    public class MenuRulesTests
    {
        [Test]
        public void Should_return_false_if_menu_id_is_not_unique()
        {
            var id = Guid.NewGuid();

            var repositoryMock = new Mock<IMenuRepository>();
            repositoryMock.Setup(x => x.GetById(id)).Returns(new Menu());

            var sut = new MenuRules(repositoryMock.Object);

            var actual = sut.IsMenuIdUnique(id);

            Assert.AreEqual(false, actual);
        }

        [Test]
        public void Should_return_true_if_menu_id_is_unique()
        {
            var id = Guid.NewGuid();

            var repositoryMock = new Mock<IMenuRepository>();
            repositoryMock.Setup(x => x.GetById(id)).Returns((Menu)null);

            var sut = new MenuRules(repositoryMock.Object);

            var actual = sut.IsMenuIdUnique(id);

            Assert.AreEqual(true, actual);
        }

        [TestCase("a@b")]
        [TestCase("a!b")]
        [TestCase("")]
        public void Should_return_false_if_menu_name_is_not_valid(string name)
        {
            var sut = new MenuRules(new Mock<IMenuRepository>().Object);
            var actual = sut.IsMenuNameValid(name);
            Assert.AreEqual(false, actual);
        }

        [TestCase("ab")]
        [TestCase("a-b")]
        [TestCase("a_b")]
        [TestCase("a1-b2")]
        [TestCase("a b")]
        public void Should_return_true_if_menu_name_is_valid(string name)
        {
            var sut = new MenuRules(new Mock<IMenuRepository>().Object);
            var actual = sut.IsMenuNameValid(name);
            Assert.AreEqual(true, actual);
        }

        [Test]
        public void Should_return_false_if_menu_name_is_not_unique()
        {
            var siteId = Guid.NewGuid();
            var name = "My Menu";

            var repositoryMock = new Mock<IMenuRepository>();
            repositoryMock.Setup(x => x.GetByName(siteId, name)).Returns(new Menu());

            var sut = new MenuRules(repositoryMock.Object);

            var actual = sut.IsMenuNameUnique(siteId, name);

            Assert.AreEqual(false, actual);
        }

        [Test]
        public void Should_return_true_if_menu_name_is_unique()
        {
            var siteId = Guid.NewGuid();
            var name = "My Menu";

            var repositoryMock = new Mock<IMenuRepository>();
            repositoryMock.Setup(x => x.GetByName(siteId, name)).Returns((Menu)null);

            var sut = new MenuRules(repositoryMock.Object);

            var actual = sut.IsMenuNameUnique(siteId, name);

            Assert.AreEqual(true, actual);
        }
    }
}
