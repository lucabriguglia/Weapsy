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
    public class UpdatePageDetailsHandlerTests
    {
        [Test]
        public void Should_throw_exception_when_page_is_not_found()
        {
            var command = new UpdatePageDetails
            {
                SiteId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Name = "Name",
                Url = "url",
                Title = "Title",
                MetaDescription = "Meta Description",
                MetaKeywords = "Meta Keywords"
            };

            var repositoryMock = new Mock<IPageRepository>();
            repositoryMock.Setup(x => x.GetById(command.SiteId, command.Id)).Returns((Page)null);

            var validatorMock = new Mock<IValidator<UpdatePageDetails>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("Url", "Url Error") }));

            var createPageHandler = new UpdatePageDetailsHandler(repositoryMock.Object, validatorMock.Object);

            Assert.Throws<Exception>(() => createPageHandler.Handle(command));
        }

        [Test]
        public void Should_throw_validation_exception_when_validation_fails()
        {
            var command = new UpdatePageDetails
            {
                SiteId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Name = "Name",
                Url = string.Empty,
                Title = "Title",
                MetaDescription = "Meta Description",
                MetaKeywords = "Meta Keywords"
            };

            var repositoryMock = new Mock<IPageRepository>();
            repositoryMock.Setup(x => x.GetById(command.SiteId, command.Id)).Returns(new Page());

            var validatorMock = new Mock<IValidator<UpdatePageDetails>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("Url", "Url Error") }));

            var createPageHandler = new UpdatePageDetailsHandler(repositoryMock.Object, validatorMock.Object);

            Assert.Throws<Exception>(() => createPageHandler.Handle(command));
        }

        [Test]
        public void Should_validate_command_and_update_page()
        {
            var command = new UpdatePageDetails
            {
                SiteId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Name = "Name",
                Url = "url",
                Title = "Title",
                MetaDescription = "Meta Description",
                MetaKeywords = "Meta Keywords"
            };

            var repositoryMock = new Mock<IPageRepository>();
            repositoryMock.Setup(x => x.GetById(command.SiteId, command.Id)).Returns(new Page());

            var validatorMock = new Mock<IValidator<UpdatePageDetails>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            var createPageHandler = new UpdatePageDetailsHandler(repositoryMock.Object, validatorMock.Object);
            createPageHandler.Handle(command);

            validatorMock.Verify(x => x.Validate(command));
            repositoryMock.Verify(x => x.Update(It.IsAny<Page>()));
        }
    }
}
