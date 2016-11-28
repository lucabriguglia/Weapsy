using System;
using System.Collections.Generic;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using NUnit.Framework;
using Weapsy.Domain.Sites;
using Weapsy.Domain.Sites.Commands;
using Weapsy.Domain.Sites.Handlers;

namespace Weapsy.Domain.Tests.Sites.Handlers
{
    [TestFixture]
    public class UpdateSiteDetailsHandlerTests
    {
        [Test]
        public void Should_throw_exception_when_site_is_not_found()
        {
            var command = new UpdateSiteDetails
            {
                SiteId = Guid.NewGuid(),
                Url = "url",
                Title = "Title",
                MetaDescription = "Meta Description",
                MetaKeywords = "Meta Keywords"
            };

            var repositoryMock = new Mock<ISiteRepository>();
            repositoryMock.Setup(x => x.GetById(command.SiteId)).Returns((Site)null);

            var validatorMock = new Mock<IValidator<UpdateSiteDetails>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("Url", "Url Error") }));

            var createSiteHandler = new UpdateSiteDetailsHandler(repositoryMock.Object, validatorMock.Object);

            Assert.Throws<Exception>(() => createSiteHandler.Handle(command));
        }

        [Test]
        public void Should_throw_validation_exception_when_validation_fails()
        {
            var command = new UpdateSiteDetails
            {
                SiteId = Guid.NewGuid(),
                Url = string.Empty,
                Title = "Title",
                MetaDescription = "Meta Description",
                MetaKeywords = "Meta Keywords"
            };

            var repositoryMock = new Mock<ISiteRepository>();
            repositoryMock.Setup(x => x.GetById(command.SiteId)).Returns(new Site());

            var validatorMock = new Mock<IValidator<UpdateSiteDetails>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("Url", "Url Error") }));

            var createSiteHandler = new UpdateSiteDetailsHandler(repositoryMock.Object, validatorMock.Object);

            Assert.Throws<Exception>(() => createSiteHandler.Handle(command));
        }

        [Test]
        public void Should_validate_command_and_update_site()
        {
            var command = new UpdateSiteDetails
            {
                SiteId = Guid.NewGuid(),
                Url = "url",
                Title = "Title",
                MetaDescription = "Meta Description",
                MetaKeywords = "Meta Keywords"
            };

            var repositoryMock = new Mock<ISiteRepository>();
            repositoryMock.Setup(x => x.GetById(command.SiteId)).Returns(new Site());

            var validatorMock = new Mock<IValidator<UpdateSiteDetails>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            var createSiteHandler = new UpdateSiteDetailsHandler(repositoryMock.Object, validatorMock.Object);
            createSiteHandler.Handle(command);

            validatorMock.Verify(x => x.Validate(command));
            repositoryMock.Verify(x => x.Update(It.IsAny<Site>()));
        }
    }
}
