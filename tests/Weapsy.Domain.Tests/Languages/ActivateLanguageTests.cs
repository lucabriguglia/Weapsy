using System.Linq;
using NUnit.Framework;
using Weapsy.Domain.Languages;
using Weapsy.Domain.Languages.Events;
using System;
using Weapsy.Domain.Languages.Commands;
using FluentValidation;
using FluentValidation.Results;
using Moq;

namespace Weapsy.Domain.Tests.Languages
{
    [TestFixture]
    public class ActivateLanguageTests
    {
        [Test]
        public void Should_call_validator()
        {
            var language = new Language();
            var command = new ActivateLanguage();
            var validatorMock = new Mock<IValidator<ActivateLanguage>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            language.Activate(command, validatorMock.Object);

            validatorMock.Verify(x => x.Validate(command));
        }

        [Test]
        public void Should_throw_exception_when_already_activated()
        {
            var language = new Language();
            var command = new ActivateLanguage();
            var validatorMock = new Mock<IValidator<ActivateLanguage>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            language.Activate(command, validatorMock.Object);

            Assert.Throws<Exception>(() => language.Activate(command, validatorMock.Object));
        }

        [Test]
        public void Should_set_language_status_to_activated()
        {
            var language = new Language();
            var command = new ActivateLanguage();
            var validatorMock = new Mock<IValidator<ActivateLanguage>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            language.Activate(command, validatorMock.Object);

            Assert.AreEqual(true, language.Status == LanguageStatus.Active);
        }

        [Test]
        public void Should_add_language_activated_event()
        {
            var language = new Language();
            var command = new ActivateLanguage();
            var validatorMock = new Mock<IValidator<ActivateLanguage>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            language.Activate(command, validatorMock.Object);

            var @event = language.Events.OfType<LanguageActivated>().SingleOrDefault();

            Assert.IsNotNull(@event);
        }

        [Test]
        public void Should_set_id_in_language_activated_event()
        {
            var language = new Language();
            var command = new ActivateLanguage();
            var validatorMock = new Mock<IValidator<ActivateLanguage>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            language.Activate(command, validatorMock.Object);

            var @event = language.Events.OfType<LanguageActivated>().SingleOrDefault();

            Assert.AreEqual(language.Id, @event.AggregateRootId);
        }

        [Test]
        public void Should_set_site_id_in_language_activated_event()
        {
            var language = new Language();
            var command = new ActivateLanguage();
            var validatorMock = new Mock<IValidator<ActivateLanguage>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            language.Activate(command, validatorMock.Object);

            var @event = language.Events.OfType<LanguageActivated>().SingleOrDefault();

            Assert.AreEqual(language.SiteId, @event.SiteId);
        }
    }
}
