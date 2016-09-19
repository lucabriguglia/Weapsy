using System;
using System.Linq;
using NUnit.Framework;
using Moq;
using FluentValidation;
using FluentValidation.Results;
using Weapsy.Domain.Languages;
using Weapsy.Domain.Languages.Commands;
using Weapsy.Domain.Languages.Events;

namespace Weapsy.Domain.Tests.Languages
{
    [TestFixture]
    public class UpdateLanguageDetailsTests
    {
        private UpdateLanguageDetails _command;
        private Mock<IValidator<UpdateLanguageDetails>> _validatorMock;
        private Language _language;
        private LanguageDetailsUpdated _event;

        [SetUp]
        public void Setup()
        {
            _command = new UpdateLanguageDetails
            {
                SiteId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Name = "My Language",
                CultureName = "aa-bb",
                Url = "url"
            };            
            _validatorMock = new Mock<IValidator<UpdateLanguageDetails>>();
            _validatorMock.Setup(x => x.Validate(_command)).Returns(new ValidationResult());
            _language = new Language();
            _language.UpdateDetails(_command, _validatorMock.Object);
            _event = _language.Events.OfType<LanguageDetailsUpdated>().SingleOrDefault();
        }

        [Test]
        public void Should_validate_command()
        {
            _validatorMock.Verify(x => x.Validate(_command));
        }

        [Test]
        public void Should_set_name()
        {
            Assert.AreEqual(_command.Name, _language.Name);
        }

        [Test]
        public void Should_set_culture_name()
        {
            Assert.AreEqual(_command.CultureName, _language.CultureName);
        }

        [Test]
        public void Should_set_url()
        {
            Assert.AreEqual(_command.Url, _language.Url);
        }

        [Test]
        public void Should_add_language_details_updated_event()
        {
            Assert.IsNotNull(_event);
        }

        [Test]
        public void Should_set_id_in_language_details_updated_event()
        {
            Assert.AreEqual(_language.Id, _event.AggregateRootId);
        }

        [Test]
        public void Should_set_site_id_in_language_details_updated_event()
        {
            Assert.AreEqual(_language.SiteId, _event.SiteId);
        }

        [Test]
        public void Should_set_name_in_language_details_updated_event()
        {
            Assert.AreEqual(_language.Name, _event.Name);
        }

        [Test]
        public void Should_set_culture_name_in_language_details_updated_event()
        {
            Assert.AreEqual(_language.CultureName, _event.CultureName);
        }

        [Test]
        public void Should_set_url_in_language_details_updated_event()
        {
            Assert.AreEqual(_language.Url, _event.Url);
        }
    }
}
