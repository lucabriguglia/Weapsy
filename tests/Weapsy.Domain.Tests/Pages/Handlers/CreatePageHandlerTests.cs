using System;
using System.Collections.Generic;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using NUnit.Framework;
using Weapsy.Domain.Pages;
using Weapsy.Domain.Pages.Commands;
using Weapsy.Domain.Pages.Handlers;

namespace Weapsy.Domain.Tests.Pages.Handlers
{
    [TestFixture]
    public class CreatePageHandlerTests
    {
        [Test]
        public void Should_throw_validation_exception_when_validation_fails()
        {
            var command = new CreatePage {
                SiteId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Name = "Name",
                Url = "url"
            };

            var repositoryMock = new Mock<IPageRepository>();

            var validatorMock = new Mock<IValidator<CreatePage>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("Id", "Id Error") }));

            var createPageHandler = new CreatePageHandler(repositoryMock.Object, validatorMock.Object);

            Assert.Throws<Exception>(() => createPageHandler.Handle(command));
        }

        [Test]
        public void Should_validate_command_and_save_new_page()
        {
            var command = new CreatePage
            {
                SiteId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Name = "Name",
                Url = "url"
            };

            var repositoryMock = new Mock<IPageRepository>();
            repositoryMock.Setup(x => x.Create(It.IsAny<Page>()));

            var validatorMock = new Mock<IValidator<CreatePage>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            var createPageHandler = new CreatePageHandler(repositoryMock.Object, validatorMock.Object);
            createPageHandler.Handle(command);

            validatorMock.Verify(x => x.Validate(command));
            repositoryMock.Verify(x => x.Create(It.IsAny<Page>()));
        }
    }
}
