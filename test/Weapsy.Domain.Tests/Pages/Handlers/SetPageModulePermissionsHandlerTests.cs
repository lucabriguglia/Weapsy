using System;
using Moq;
using NUnit.Framework;
using Weapsy.Domain.Pages;
using Weapsy.Domain.Pages.Commands;
using Weapsy.Domain.Pages.Handlers;
using System.Collections.Generic;
using Weapsy.Tests.Factories;

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
                Id = Guid.NewGuid(),
                PageModuleId = Guid.NewGuid(),
                PageModulePermissions = new List<PageModulePermission>()
            };

            var page = PageFactory.Page(command.SiteId, command.Id, "My Page", command.PageModuleId);

            var repositoryMock = new Mock<IPageRepository>();
            repositoryMock.Setup(x => x.GetById(command.SiteId, command.Id)).Returns(page);

            var setPageModulePermissionsHandler = new SetPageModulePermissionsHandler(repositoryMock.Object);

            setPageModulePermissionsHandler.Handle(command);

            repositoryMock.Verify(x => x.Update(It.IsAny<Page>()));
        }
    }
}
