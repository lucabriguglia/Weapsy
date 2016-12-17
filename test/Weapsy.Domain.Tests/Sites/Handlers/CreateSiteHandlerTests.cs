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
    public class CreateSiteHandlerTests
    {
        [Test]
        public void Should_throw_validation_exception_when_validation_fails()
        {
            var command = new CreateSite
            {
                Id = Guid.NewGuid(),
                Name = "Name"
            };

            var siteRepositoryMock = new Mock<ISiteRepository>();

            var validatorMock = new Mock<IValidator<CreateSite>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("Id", "Id Error") }));

            var createSiteHandler = new CreateSiteHandler(siteRepositoryMock.Object, validatorMock.Object);

            Assert.Throws<Exception>(() => createSiteHandler.Handle(command));
        }

        [Test]
        public void Should_validate_command_and_save_new_site()
        {
            var command = new CreateSite
            {
                Id = Guid.NewGuid(),
                Name = "Name"
            };

            var siteRepositoryMock = new Mock<ISiteRepository>();
            siteRepositoryMock.Setup(x => x.Create(It.IsAny<Site>()));

            var validatorMock = new Mock<IValidator<CreateSite>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            var createSiteHandler = new CreateSiteHandler(siteRepositoryMock.Object, validatorMock.Object);
            createSiteHandler.Handle(command);

            validatorMock.Verify(x => x.Validate(command));
            siteRepositoryMock.Verify(x => x.Create(It.IsAny<Site>()));
        }
    }
}
