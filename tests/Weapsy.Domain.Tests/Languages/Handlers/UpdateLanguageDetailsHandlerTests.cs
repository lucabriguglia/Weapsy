using System;
using System.Collections.Generic;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using NUnit.Framework;
using Weapsy.Domain.Languages;
using Weapsy.Domain.Languages.Commands;
using Weapsy.Domain.Languages.Handlers;

namespace Weapsy.Domain.Tests.Languages.Handlers
{
    [TestFixture]
    public class UpdateLanguageDetailsHandlerTests
    {
        [Test]
        public void Should_throw_exception_when_language_is_not_found()
        {
            var command = new UpdateLanguageDetails
            {
                SiteId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Name = "My Language",
                CultureName = "aa-bb",
                Url = "url"
            };

            var languageRepositoryMock = new Mock<ILanguageRepository>();
            languageRepositoryMock.Setup(x => x.GetById(command.SiteId, command.Id)).Returns((Language)null);

            var validatorMock = new Mock<IValidator<UpdateLanguageDetails>>();

            var createLanguageHandler = new UpdateLanguageDetailsHandler(languageRepositoryMock.Object, validatorMock.Object);

            Assert.Throws<Exception>(() => createLanguageHandler.Handle(command));
        }

        [Test]
        public void Should_throw_validation_exception_when_validation_fails()
        {
            var command = new UpdateLanguageDetails
            {
                SiteId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Name = "My Language",
                CultureName = "aa-bb",
                Url = "url"
            };

            var languageRepositoryMock = new Mock<ILanguageRepository>();
            languageRepositoryMock.Setup(x => x.GetById(command.SiteId, command.Id)).Returns(new Language());

            var validatorMock = new Mock<IValidator<UpdateLanguageDetails>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("Name", "Name Error") }));

            var createLanguageHandler = new UpdateLanguageDetailsHandler(languageRepositoryMock.Object, validatorMock.Object);

            Assert.Throws<Exception>(() => createLanguageHandler.Handle(command));
        }

        [Test]
        public void Should_validate_command_and_save_new_language()
        {
            var command = new UpdateLanguageDetails
            {
                SiteId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Name = "My Language",
                CultureName = "aa-bb",
                Url = "url"
            };

            var languageRepositoryMock = new Mock<ILanguageRepository>();
            languageRepositoryMock.Setup(x => x.GetById(command.SiteId, command.Id)).Returns(new Language());
            languageRepositoryMock.Setup(x => x.Update(It.IsAny<Language>()));

            var validatorMock = new Mock<IValidator<UpdateLanguageDetails>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            var createLanguageHandler = new UpdateLanguageDetailsHandler(languageRepositoryMock.Object, validatorMock.Object);
            createLanguageHandler.Handle(command);

            validatorMock.Verify(x => x.Validate(command));
            languageRepositoryMock.Verify(x => x.Update(It.IsAny<Language>()));
        }
    }
}
