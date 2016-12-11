using System.Linq;
using NUnit.Framework;
using Weapsy.Domain.Languages;
using Weapsy.Domain.Languages.Events;
using System;
using Weapsy.Domain.Languages.Commands;
using Moq;
using FluentValidation.Results;
using FluentValidation;

namespace Weapsy.Domain.Tests.Languages
{
    [TestFixture]
    public class DeleteLanguageTests
    {
        [Test]
        public void Should_call_validator()
        {
            var language = new Language();
            var command = new DeleteLanguage();
            var validatorMock = new Mock<IValidator<DeleteLanguage>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            language.Delete(command, validatorMock.Object);

            validatorMock.Verify(x => x.Validate(command));
        }

        [Test]
        public void Should_throw_exception_when_already_deleted()
        {
            var language = new Language();
            var command = new DeleteLanguage();
            var validatorMock = new Mock<IValidator<DeleteLanguage>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            language.Delete(command, validatorMock.Object);

            Assert.Throws<Exception>(() => language.Delete(command, validatorMock.Object));
        }

        [Test]
        public void Should_set_language_status_to_deleted()
        {
            var language = new Language();
            var command = new DeleteLanguage();
            var validatorMock = new Mock<IValidator<DeleteLanguage>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            language.Delete(command, validatorMock.Object);

            Assert.AreEqual(true, language.Status == LanguageStatus.Deleted);
        }

        [Test]
        public void Should_add_language_deleted_event()
        {
            var language = new Language();
            var command = new DeleteLanguage();
            var validatorMock = new Mock<IValidator<DeleteLanguage>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            language.Delete(command, validatorMock.Object);

            var @event = language.Events.OfType<LanguageDeleted>().SingleOrDefault();

            Assert.IsNotNull(@event);
        }

        [Test]
        public void Should_set_id_in_language_deleted_event()
        {
            var language = new Language();
            var command = new DeleteLanguage();
            var validatorMock = new Mock<IValidator<DeleteLanguage>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            language.Delete(command, validatorMock.Object);

            var @event = language.Events.OfType<LanguageDeleted>().SingleOrDefault();

            Assert.AreEqual(language.Id, @event.AggregateRootId);
        }

        [Test]
        public void Should_set_site_id_in_language_deleted_event()
        {
            var language = new Language();
            var command = new DeleteLanguage();
            var validatorMock = new Mock<IValidator<DeleteLanguage>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            language.Delete(command, validatorMock.Object);

            var @event = language.Events.OfType<LanguageDeleted>().SingleOrDefault();

            Assert.AreEqual(language.SiteId, @event.SiteId);
        }
    }
}