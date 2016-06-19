using System;
using System.Linq;
using NUnit.Framework;
using Moq;
using FluentValidation;
using FluentValidation.Results;
using Weapsy.Domain.Model.Languages;
using Weapsy.Domain.Model.Languages.Commands;
using Weapsy.Domain.Model.Languages.Events;

namespace Weapsy.Domain.Tests.Languages
{
    [TestFixture]
    public class UpdateLanguageDetailsTests
    {
        private UpdateLanguageDetails command;
        private Mock<IValidator<UpdateLanguageDetails>> validatorMock;
        private Language language;
        private LanguageDetailsUpdated @event;

        [SetUp]
        public void Setup()
        {
            command = new UpdateLanguageDetails
            {
                SiteId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Name = "My Language",
                CultureName = "aa-bb",
                Url = "url"
            };            
            validatorMock = new Mock<IValidator<UpdateLanguageDetails>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());
            language = new Language();
            language.UpdateDetails(command, validatorMock.Object);
            @event = language.Events.OfType<LanguageDetailsUpdated>().SingleOrDefault();
        }

        [Test]
        public void Should_validate_command()
        {
            validatorMock.Verify(x => x.Validate(command));
        }

        [Test]
        public void Should_set_name()
        {
            Assert.AreEqual(command.Name, language.Name);
        }

        [Test]
        public void Should_set_culture_name()
        {
            Assert.AreEqual(command.CultureName, language.CultureName);
        }

        [Test]
        public void Should_set_url()
        {
            Assert.AreEqual(command.Url, language.Url);
        }

        [Test]
        public void Should_add_language_details_updated_event()
        {
            Assert.IsNotNull(@event);
        }

        [Test]
        public void Should_set_id_in_language_details_updated_event()
        {
            Assert.AreEqual(language.Id, @event.AggregateRootId);
        }

        [Test]
        public void Should_set_site_id_in_language_details_updated_event()
        {
            Assert.AreEqual(language.SiteId, @event.SiteId);
        }

        [Test]
        public void Should_set_name_in_language_details_updated_event()
        {
            Assert.AreEqual(language.Name, @event.Name);
        }

        [Test]
        public void Should_set_culture_name_in_language_details_updated_event()
        {
            Assert.AreEqual(language.CultureName, @event.CultureName);
        }

        [Test]
        public void Should_set_url_in_language_details_updated_event()
        {
            Assert.AreEqual(language.Url, @event.Url);
        }
    }
}
