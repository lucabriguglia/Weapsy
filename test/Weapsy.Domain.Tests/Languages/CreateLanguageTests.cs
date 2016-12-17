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
    public class CreateLanguageTests
    {
        private CreateLanguage _command;
        private Mock<IValidator<CreateLanguage>> _validatorMock;
        private Mock<ILanguageSortOrderGenerator> _sortOrderGeneratorMock;
        private Language _language;
        private LanguageCreated _event;

        [SetUp]
        public void Setup()
        {
            _command = new CreateLanguage
            {
                SiteId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Name = "My Language",
                CultureName = "aa-bb",
                Url = "url"
            };            
            _validatorMock = new Mock<IValidator<CreateLanguage>>();
            _validatorMock.Setup(x => x.Validate(_command)).Returns(new ValidationResult());
            _sortOrderGeneratorMock = new Mock<ILanguageSortOrderGenerator>();
            _sortOrderGeneratorMock.Setup(x => x.GenerateNextSortOrder(_command.SiteId)).Returns(4);
            _language = Language.CreateNew(_command, _validatorMock.Object, _sortOrderGeneratorMock.Object);
            _event = _language.Events.OfType<LanguageCreated>().SingleOrDefault();
        }

        [Test]
        public void Should_validate_command()
        {
            _validatorMock.Verify(x => x.Validate(_command));
        }

        [Test]
        public void Should_set_id()
        {
            Assert.AreEqual(_command.Id, _language.Id);
        }

        [Test]
        public void Should_set_site_id()
        {
            Assert.AreEqual(_command.SiteId, _language.SiteId);
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
        public void Should_call_sort_order_generator()
        {
            _sortOrderGeneratorMock.Verify(x => x.GenerateNextSortOrder(_command.SiteId));
        }

        [Test]
        public void Should_set_sort_order()
        {
            Assert.AreEqual(4, _language.SortOrder);
        }

        [Test]
        public void Should_set_status_to_hidden()
        {
            Assert.AreEqual(LanguageStatus.Hidden, _language.Status);
        }

        [Test]
        public void Should_add_language_created_event()
        {
            Assert.IsNotNull(_event);
        }

        [Test]
        public void Should_set_id_in_language_created_event()
        {
            Assert.AreEqual(_language.Id, _event.AggregateRootId);
        }

        [Test]
        public void Should_set_site_id_in_language_created_event()
        {
            Assert.AreEqual(_language.SiteId, _event.SiteId);
        }

        [Test]
        public void Should_set_name_in_language_created_event()
        {
            Assert.AreEqual(_language.Name, _event.Name);
        }

        [Test]
        public void Should_set_culture_name_in_language_created_event()
        {
            Assert.AreEqual(_language.CultureName, _event.CultureName);
        }

        [Test]
        public void Should_set_url_in_language_created_event()
        {
            Assert.AreEqual(_language.Url, _event.Url);
        }

        [Test]
        public void Should_set_status_in_language_created_event()
        {
            Assert.AreEqual(_language.Status, _event.Status);
        }
    }
}
