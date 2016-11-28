using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

            Assert.ThrowsAsync<Exception>(async () => await createLanguageHandler.HandleAsync(command));
        }

        [Test]
        public async Task Should_validate_command_and_save_new_language()
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
            languageRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<Language>())).Returns(Task.FromResult(false));

            var validatorMock = new Mock<IValidator<CreateLanguage>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            var sortOrderGeneratorMock = new Mock<ILanguageSortOrderGenerator>();

            var createLanguageHandler = new CreateLanguageHandler(languageRepositoryMock.Object, validatorMock.Object, sortOrderGeneratorMock.Object);
            await createLanguageHandler.HandleAsync(command);

            validatorMock.Verify(x => x.Validate(command));
            languageRepositoryMock.Verify(x => x.CreateAsync(It.IsAny<Language>()));
        }
    }
}
