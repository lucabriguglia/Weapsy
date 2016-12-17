using System;
using Moq;
using NUnit.Framework;
using Weapsy.Domain.Languages;
using Weapsy.Domain.Languages.Commands;
using Weapsy.Domain.Languages.Handlers;
using FluentValidation;
using FluentValidation.Results;

namespace Weapsy.Domain.Tests.Languages.Handlers
{
    [TestFixture]
    public class HideLanguageHandlerTests
    {
        [Test]
        public void Should_throw_exception_when_language_is_not_found()
        {
            var command = new HideLanguage
            {
                SiteId = Guid.NewGuid(),
                Id = Guid.NewGuid()
            };

            var repositoryMock = new Mock<ILanguageRepository>();
            repositoryMock.Setup(x => x.GetById(command.SiteId, command.Id)).Returns((Language)null);

            var validatorMock = new Mock<IValidator<HideLanguage>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            var hideLanguageHandler = new HideLanguageHandler(repositoryMock.Object, validatorMock.Object);

            Assert.Throws<Exception>(() => hideLanguageHandler.Handle(command));
        }

        [Test]
        public void Should_update_language()
        {
            var command = new HideLanguage
            {
                SiteId = Guid.NewGuid(),
                Id = Guid.NewGuid()
            };

            var languageMock = new Mock<Language>();

            var repositoryMock = new Mock<ILanguageRepository>();
            repositoryMock.Setup(x => x.GetById(command.SiteId, command.Id)).Returns(languageMock.Object);

            var validatorMock = new Mock<IValidator<HideLanguage>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            var hideLanguageHandler = new HideLanguageHandler(repositoryMock.Object, validatorMock.Object);

            hideLanguageHandler.Handle(command);

            repositoryMock.Verify(x => x.Update(It.IsAny<Language>()));
        }
    }
}
