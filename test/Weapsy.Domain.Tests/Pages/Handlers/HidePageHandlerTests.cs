using System;
using Moq;
using NUnit.Framework;
using Weapsy.Domain.Pages;
using Weapsy.Domain.Pages.Commands;
using Weapsy.Domain.Pages.Handlers;
using FluentValidation.Results;
using FluentValidation;

namespace Weapsy.Domain.Tests.Pages.Handlers
{
    [TestFixture]
    public class HidePageHandlerTests
    {
        [Test]
        public void Should_throw_exception_when_page_is_not_found()
        {
            var command = new HidePage
            {
                SiteId = Guid.NewGuid(),
                Id = Guid.NewGuid()
            };

            var repositoryMock = new Mock<IPageRepository>();
            repositoryMock.Setup(x => x.GetById(command.SiteId, command.Id)).Returns((Page)null);

            var validatorMock = new Mock<IValidator<HidePage>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            var hidePageHandler = new HidePageHandler(repositoryMock.Object, validatorMock.Object);

            Assert.Throws<Exception>(() => hidePageHandler.Handle(command));
        }

        [Test]
        public void Should_update_page()
        {
            var command = new HidePage
            {
                SiteId = Guid.NewGuid(),
                Id = Guid.NewGuid()
            };

            var pageMock = new Mock<Page>();

            var repositoryMock = new Mock<IPageRepository>();
            repositoryMock.Setup(x => x.GetById(command.SiteId, command.Id)).Returns(pageMock.Object);

            var validatorMock = new Mock<IValidator<HidePage>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            var hidePageHandler = new HidePageHandler(repositoryMock.Object, validatorMock.Object);

            hidePageHandler.Handle(command);

            repositoryMock.Verify(x => x.Update(It.IsAny<Page>()));
        }
    }
}
