using System.Linq;
using NUnit.Framework;
using Weapsy.Domain.Languages;
using Weapsy.Domain.Languages.Events;
using System;
using FluentValidation.Results;
using Weapsy.Domain.Languages.Commands;
using Moq;
using FluentValidation;

namespace Weapsy.Domain.Tests.Languages
{
    [TestFixture]
    public class HideLanguageTests
    {
        [Test]
        public void Should_call_validator()
        {
            var language = new Language();
            var command = new HideLanguage();
            var validatorMock = new Mock<IValidator<HideLanguage>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            language.Hide(command, validatorMock.Object);

            validatorMock.Verify(x => x.Validate(command));
        }

        [Test]
        public void Should_throw_exception_when_already_hidden()
        {
            var language = new Language();
            var command = new HideLanguage();
            var validatorMock = new Mock<IValidator<HideLanguage>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            language.Hide(command, validatorMock.Object);

            Assert.Throws<Exception>(() => language.Hide(command, validatorMock.Object));
        }

        [Test]
        public void Should_throw_exception_when_deleted()
        {
            var language = new Language();
            var command = new HideLanguage();
            var validatorMock = new Mock<IValidator<HideLanguage>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            var deleteLanguageCommand = new DeleteLanguage
            {
                SiteId = language.SiteId,
                Id = language.Id
            };
            var deleteLanguageValidatorMock = new Mock<IValidator<DeleteLanguage>>();
            deleteLanguageValidatorMock.Setup(x => x.Validate(deleteLanguageCommand)).Returns(new ValidationResult());
            language.Delete(deleteLanguageCommand, deleteLanguageValidatorMock.Object);

            Assert.Throws<Exception>(() => language.Hide(command, validatorMock.Object));
        }

        [Test]
        public void Should_set_language_status_to_hidden()
        {
            var language = new Language();
            var command = new HideLanguage();
            var validatorMock = new Mock<IValidator<HideLanguage>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            language.Hide(command, validatorMock.Object);

            Assert.AreEqual(true, language.Status == LanguageStatus.Hidden);
        }

        [Test]
        public void Should_add_language_hidden_event()
        {
            var language = new Language();
            var command = new HideLanguage();
            var validatorMock = new Mock<IValidator<HideLanguage>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            language.Hide(command, validatorMock.Object);

            var @event = language.Events.OfType<LanguageHidden>().SingleOrDefault();

            Assert.IsNotNull(@event);
        }

        [Test]
        public void Should_set_id_in_language_hidden_event()
        {
            var language = new Language();
            var command = new HideLanguage();
            var validatorMock = new Mock<IValidator<HideLanguage>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            language.Hide(command, validatorMock.Object);

            var @event = language.Events.OfType<LanguageHidden>().SingleOrDefault();

            Assert.AreEqual(language.Id, @event.AggregateRootId);
        }

        [Test]
        public void Should_set_site_id_in_language_hidden_event()
        {
            var language = new Language();
            var command = new HideLanguage();
            var validatorMock = new Mock<IValidator<HideLanguage>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            language.Hide(command, validatorMock.Object);

            var @event = language.Events.OfType<LanguageHidden>().SingleOrDefault();

            Assert.AreEqual(language.SiteId, @event.SiteId);
        }
    }
}
