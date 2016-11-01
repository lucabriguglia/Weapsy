using System;
using Moq;
using NUnit.Framework;
using Weapsy.Domain.Modules;
using Weapsy.Domain.Modules.Rules;

namespace Weapsy.Domain.Tests.Modules.Rules
{
    [TestFixture]
    public class ModuleRulesTests
    {
        [Test]
        public void Should_return_false_if_module_does_not_exists()
        {
            var id = Guid.NewGuid();
            var siteId = Guid.NewGuid();

            var repositoryMock = new Mock<IModuleRepository>();
            repositoryMock.Setup(x => x.GetById(siteId, id)).Returns((Module)null);

            var sut = new ModuleRules(repositoryMock.Object);

            var actual = sut.DoesModuleExist(siteId, id);

            Assert.AreEqual(false, actual);
        }

        [Test]
        public void Should_return_true_if_module_exists()
        {
            var id = Guid.NewGuid();
            var siteId = Guid.NewGuid();

            var repositoryMock = new Mock<IModuleRepository>();
            repositoryMock.Setup(x => x.GetById(siteId, id)).Returns(new Module());

            var sut = new ModuleRules(repositoryMock.Object);

            var actual = sut.DoesModuleExist(siteId, id);

            Assert.AreEqual(true, actual);
        }

        [Test]
        public void Should_return_false_if_module_id_is_not_unique()
        {
            var id = Guid.NewGuid();

            var repositoryMock = new Mock<IModuleRepository>();
            repositoryMock.Setup(x => x.GetById(id)).Returns(new Module());

            var sut = new ModuleRules(repositoryMock.Object);

            var actual = sut.IsModuleIdUnique(id);

            Assert.AreEqual(false, actual);
        }

        [Test]
        public void Should_return_true_if_module_id_is_unique()
        {
            var id = Guid.NewGuid();

            var repositoryMock = new Mock<IModuleRepository>();
            repositoryMock.Setup(x => x.GetById(id)).Returns((Module)null);

            var sut = new ModuleRules(repositoryMock.Object);

            var actual = sut.IsModuleIdUnique(id);

            Assert.AreEqual(true, actual);
        }
    }
}
