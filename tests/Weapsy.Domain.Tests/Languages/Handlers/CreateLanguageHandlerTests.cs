using System;
using System.Collections.Generic;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using NUnit.Framework;
using Weapsy.Domain.Model.Languages;
using Weapsy.Domain.Model.Languages.Commands;
using Weapsy.Domain.Model.Languages.Handlers;

namespace Weapsy.Domain.Tests.Languages.Handlers
{
    [TestFixture]
    public class CreateLanguageHandlerTests
    {
        [Test]
        public void Should_throw_validation_exception_when_validation_fails()
        {
            var command = new CreateLanguage
            {
                SiteId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Name = "My Language",
                CultureName = "aa-bb",
                Url = "url"
            };

            var languageRepositoryMock = new Mock<ILanguageRepository>();

            var validatorMock = new Mock<IValidator<CreateLanguage>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("Id", "Id Error") }));

            var sortOrderGeneratorMock = new Mock<ILanguageSortOrderGenerator>();

            var createLanguageHandler = new CreateLanguageHandler(languageRepositoryMock.Object, validatorMock.Object, sortOrderGeneratorMock.Object);

            Assert.Throws<ValidationException>(() => createLanguageHandler.Handle(command));
        }

        [Test]
        public void Should_validate_command_and_save_new_language()
        {
            var command = new CreateLanguage
            {
                SiteId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Name = "My Language",
                CultureName = "aa-bb",
                Url = "url"
            };

            var languageRepositoryMock = new Mock<ILanguageRepository>();
            languageRepositoryMock.Setup(x => x.Create(It.IsAny<Language>()));

            var validatorMock = new Mock<IValidator<CreateLanguage>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            var sortOrderGeneratorMock = new Mock<ILanguageSortOrderGenerator>();

            var createLanguageHandler = new CreateLanguageHandler(languageRepositoryMock.Object, validatorMock.Object, sortOrderGeneratorMock.Object);
            createLanguageHandler.Handle(command);

            validatorMock.Verify(x => x.Validate(command));
            languageRepositoryMock.Verify(x => x.Create(It.IsAny<Language>()));
        }
    }
}
