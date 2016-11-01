using System;
using Moq;
using NUnit.Framework;
using Weapsy.Domain.Apps;
using Weapsy.Domain.Apps.Rules;

namespace Weapsy.Domain.Tests.Apps.Rules
{
    [TestFixture]
    public class AppRulesTests
    {
        [Test]
        public void Should_return_false_if_app_not_exists()
        {
            var id = Guid.NewGuid();

            var appRepositoryMock = new Mock<IAppRepository>();
            appRepositoryMock.Setup(x => x.GetById(id)).Returns((App)null);

            var sut = new AppRules(appRepositoryMock.Object);

            var actual = sut.DoesAppExist(id);

            Assert.AreEqual(false, actual);
        }

        [Test]
        public void Should_return_true_if_app_exists()
        {
            var id = Guid.NewGuid();

            var appRepositoryMock = new Mock<IAppRepository>();
            appRepositoryMock.Setup(x => x.GetById(id)).Returns(new App());

            var sut = new AppRules(appRepositoryMock.Object);

            var actual = sut.DoesAppExist(id);

            Assert.AreEqual(true, actual);
        }

        [Test]
        public void Should_return_false_if_app_id_is_not_unique()
        {
            var id = Guid.NewGuid();

            var appRepositoryMock = new Mock<IAppRepository>();
            appRepositoryMock.Setup(x => x.GetById(id)).Returns(new App());

            var sut = new AppRules(appRepositoryMock.Object);

            var actual = sut.IsAppIdUnique(id);

            Assert.AreEqual(false, actual);
        }

        [Test]
        public void Should_return_true_if_app_id_is_unique()
        {
            var id = Guid.NewGuid();

            var appRepositoryMock = new Mock<IAppRepository>();
            appRepositoryMock.Setup(x => x.GetById(id)).Returns((App)null);

            var sut = new AppRules(appRepositoryMock.Object);

            var actual = sut.IsAppIdUnique(id);

            Assert.AreEqual(true, actual);
        }

        [Test]
        public void Should_return_false_if_app_name_is_not_unique()
        {
            var name = "My App";

            var appRepositoryMock = new Mock<IAppRepository>();
            appRepositoryMock.Setup(x => x.GetByName(name)).Returns(new App());

            var sut = new AppRules(appRepositoryMock.Object);

            var actual = sut.IsAppNameUnique(name);

            Assert.AreEqual(false, actual);
        }

        [Test]
        public void Should_return_true_if_app_name_is_unique()
        {
            var name = "My App";

            var appRepositoryMock = new Mock<IAppRepository>();
            appRepositoryMock.Setup(x => x.GetByName(name)).Returns((App)null);

            var sut = new AppRules(appRepositoryMock.Object);

            var actual = sut.IsAppNameUnique(name);

            Assert.AreEqual(true, actual);
        }

        [Test]
        public void Should_return_false_if_app_folder_is_not_valid()
        {
            var folder = "My@App";

            var appRepositoryMock = new Mock<IAppRepository>();
            var sut = new AppRules(appRepositoryMock.Object);

            var actual = sut.IsAppFolderValid(folder);

            Assert.AreEqual(false, actual);
        }

        [Test]
        public void Should_return_true_if_app_folder_is_valid()
        {
            var folder = "MyApp";

            var appRepositoryMock = new Mock<IAppRepository>();
            var sut = new AppRules(appRepositoryMock.Object);

            var actual = sut.IsAppFolderValid(folder);

            Assert.AreEqual(true, actual);
        }

        [Test]
        public void Should_return_false_if_app_folder_is_not_unique()
        {
            var folder = "MyApp";

            var appRepositoryMock = new Mock<IAppRepository>();
            appRepositoryMock.Setup(x => x.GetByFolder(folder)).Returns(new App());

            var sut = new AppRules(appRepositoryMock.Object);

            var actual = sut.IsAppFolderUnique(folder);

            Assert.AreEqual(false, actual);
        }

        [Test]
        public void Should_return_true_if_app_folder_is_unique()
        {
            var folder = "MyApp";

            var appRepositoryMock = new Mock<IAppRepository>();
            appRepositoryMock.Setup(x => x.GetByFolder(folder)).Returns((App)null);

            var sut = new AppRules(appRepositoryMock.Object);

            var actual = sut.IsAppFolderUnique(folder);

            Assert.AreEqual(true, actual);
        }
    }
}
