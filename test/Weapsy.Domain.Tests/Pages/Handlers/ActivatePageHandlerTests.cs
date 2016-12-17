using System;
using Moq;
using NUnit.Framework;
using Weapsy.Domain.Pages;
using Weapsy.Domain.Pages.Commands;
using Weapsy.Domain.Pages.Handlers;
using FluentValidation;
using FluentValidation.Results;

namespace Weapsy.Domain.Tests.Pages.Handlers
{
    [TestFixture]
    public class ActivatePageHandlerTests
    {
        [Test]
        public void Should_throw_exception_when_page_is_not_found()
        {
            var command = new ActivatePage
            {
                SiteId = Guid.NewGuid(),
                Id = Guid.NewGuid()
            };

            var repositoryMock = new Mock<IPageRepository>();
            repositoryMock.Setup(x => x.GetById(command.SiteId, command.Id)).Returns((Page)null);

            var validatorMock = new Mock<IValidator<ActivatePage>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            var activatePageHandler = new ActivatePageHandler(repositoryMock.Object, validatorMock.Object);

            Assert.Throws<Exception>(() => activatePageHandler.Handle(command));
        }

        [Test]
        public void Should_update_page()
        {
            var command = new ActivatePage
            {
                SiteId = Guid.NewGuid(),
                Id = Guid.NewGuid()
            };

            var pageMock = new Mock<Page>();

            var repositoryMock = new Mock<IPageRepository>();
            repositoryMock.Setup(x => x.GetById(command.SiteId, command.Id)).Returns(pageMock.Object);

            var validatorMock = new Mock<IValidator<ActivatePage>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            var activatePageHandler = new ActivatePageHandler(repositoryMock.Object, validatorMock.Object);

            activatePageHandler.Handle(command);

            repositoryMock.Verify(x => x.Update(It.IsAny<Page>()));
        }
    }
}
