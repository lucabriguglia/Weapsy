using System;
using Moq;
using NUnit.Framework;
using Weapsy.Domain.Model.Pages;
using Weapsy.Domain.Model.Pages.Commands;
using Weapsy.Domain.Model.Pages.Handlers;

namespace Weapsy.Domain.Tests.Pages.Handlers
{
    [TestFixture]
    public class SetPageModulePermissionsHandlerTests
    {
        [Test]
        public void Should_throw_exception_when_page_is_not_found()
        {
            var command = new SetPageModulePermissions
            {
                SiteId = Guid.NewGuid(),
                Id = Guid.NewGuid()
            };

            var repositoryMock = new Mock<IPageRepository>();
            repositoryMock.Setup(x => x.GetById(command.SiteId, command.Id)).Returns((Page)null);

            var setPageModulePermissionsHandler = new SetPageModulePermissionsHandler(repositoryMock.Object);

            Assert.Throws<Exception>(() => setPageModulePermissionsHandler.Handle(command));
        }

        [Test]
        public void Should_update_page()
        {
            var command = new SetPageModulePermissions
            {
                SiteId = Guid.NewGuid(),
                Id = Guid.NewGuid()
            };

            var pageMock = new Mock<Page>();

            var repositoryMock = new Mock<IPageRepository>();
            repositoryMock.Setup(x => x.GetById(command.SiteId, command.Id)).Returns(pageMock.Object);

            var setPageModulePermissionsHandler = new SetPageModulePermissionsHandler(repositoryMock.Object);

            setPageModulePermissionsHandler.Handle(command);

            repositoryMock.Verify(x => x.Update(It.IsAny<Page>()));
        }
    }
}
