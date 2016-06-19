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
    public class CreateLanguageTests
    {
        private CreateLanguage command;
        private Mock<IValidator<CreateLanguage>> validatorMock;
        private Mock<ILanguageSortOrderGenerator> sortOrderGeneratorMock;
        private Language language;
        private LanguageCreated @event;

        [SetUp]
        public void Setup()
        {
            command = new CreateLanguage
            {
                SiteId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                Name = "My Language",
                CultureName = "aa-bb",
                Url = "url"
            };            
            validatorMock = new Mock<IValidator<CreateLanguage>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());
            sortOrderGeneratorMock = new Mock<ILanguageSortOrderGenerator>();
            sortOrderGeneratorMock.Setup(x => x.GenerateNextSortOrder(command.SiteId)).Returns(4);
            language = Language.CreateNew(command, validatorMock.Object, sortOrderGeneratorMock.Object);
            @event = language.Events.OfType<LanguageCreated>().SingleOrDefault();
        }

        [Test]
        public void Should_validate_command()
        {
            validatorMock.Verify(x => x.Validate(command));
        }

        [Test]
        public void Should_set_id()
        {
            Assert.AreEqual(command.Id, language.Id);
        }

        [Test]
        public void Should_set_site_id()
        {
            Assert.AreEqual(command.SiteId, language.SiteId);
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
        public void Should_call_sort_order_generator()
        {
            sortOrderGeneratorMock.Verify(x => x.GenerateNextSortOrder(command.SiteId));
        }

        [Test]
        public void Should_set_sort_order()
        {
            Assert.AreEqual(4, language.SortOrder);
        }

        [Test]
        public void Should_set_status_to_hidden()
        {
            Assert.AreEqual(LanguageStatus.Hidden, language.Status);
        }

        [Test]
        public void Should_add_language_created_event()
        {
            Assert.IsNotNull(@event);
        }

        [Test]
        public void Should_set_id_in_language_created_event()
        {
            Assert.AreEqual(language.Id, @event.AggregateRootId);
        }

        [Test]
        public void Should_set_site_id_in_language_created_event()
        {
            Assert.AreEqual(language.SiteId, @event.SiteId);
        }

        [Test]
        public void Should_set_name_in_language_created_event()
        {
            Assert.AreEqual(language.Name, @event.Name);
        }

        [Test]
        public void Should_set_culture_name_in_language_created_event()
        {
            Assert.AreEqual(language.CultureName, @event.CultureName);
        }

        [Test]
        public void Should_set_url_in_language_created_event()
        {
            Assert.AreEqual(language.Url, @event.Url);
        }

        [Test]
        public void Should_set_status_in_language_created_event()
        {
            Assert.AreEqual(language.Status, @event.Status);
        }
    }
}
