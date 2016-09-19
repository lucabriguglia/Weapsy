using System;
using Moq;
using NUnit.Framework;
using Weapsy.Domain.Sites;
using Weapsy.Domain.Sites.Commands;
using Weapsy.Domain.Sites.Handlers;

namespace Weapsy.Domain.Tests.Sites.Handlers
{
    [TestFixture]
    public class CloseSiteHandlerTests
    {
        [Test]
        public void Should_throw_exception_when_site_is_not_found()
        {
            var command = new CloseSite
            {
                Id = Guid.NewGuid()
            };

            var repositoryMock = new Mock<ISiteRepository>();
            repositoryMock.Setup(x => x.GetById(command.Id)).Returns((Site)null);

            var closeSiteHandler = new CloseSiteHandler(repositoryMock.Object);

            Assert.Throws<Exception>(() => closeSiteHandler.Handle(command));
        }

        [Test]
        public void Should_update_site()
        {
            var command = new CloseSite
            {
                Id = Guid.NewGuid()
            };

            var siteMock = new Mock<Site>();

            var repositoryMock = new Mock<ISiteRepository>();
            repositoryMock.Setup(x => x.GetById(command.Id)).Returns(siteMock.Object);

            var closeSiteHandler = new CloseSiteHandler(repositoryMock.Object);

            closeSiteHandler.Handle(command);

            repositoryMock.Verify(x => x.Update(It.IsAny<Site>()));
        }
    }
}
