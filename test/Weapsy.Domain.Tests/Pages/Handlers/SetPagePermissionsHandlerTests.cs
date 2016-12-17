using System;
using Moq;
using NUnit.Framework;
using Weapsy.Domain.Pages;
using Weapsy.Domain.Pages.Commands;
using Weapsy.Domain.Pages.Handlers;
using Weapsy.Tests.Factories;
using System.Collections.Generic;

namespace Weapsy.Domain.Tests.Pages.Handlers
{
    [TestFixture]
    public class SetPagePermissionsHandlerTests
    {
        [Test]
        public void Should_throw_exception_when_page_is_not_found()
        {
            var command = new SetPagePermissions
            {
                SiteId = Guid.NewGuid(),
                Id = Guid.NewGuid()
            };

            var repositoryMock = new Mock<IPageRepository>();
            repositoryMock.Setup(x => x.GetById(command.SiteId, command.Id)).Returns((Page)null);

            var setPagePermissionsHandler = new SetPagePermissionsHandler(repositoryMock.Object);

            Assert.Throws<Exception>(() => setPagePermissionsHandler.Handle(command));
        }

        [Test]
        public void Should_update_page()
        {
            var command = new SetPagePermissions
            {
                SiteId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                PagePermissions = new List<PagePermission>()
            };

            var page = PageFactory.Page(command.SiteId, command.Id, "My Page");

            var repositoryMock = new Mock<IPageRepository>();
            repositoryMock.Setup(x => x.GetById(command.SiteId, command.Id)).Returns(page);

            var setPagePermissionsHandler = new SetPagePermissionsHandler(repositoryMock.Object);

            setPagePermissionsHandler.Handle(command);

            repositoryMock.Verify(x => x.Update(It.IsAny<Page>()));
        }
    }
}
