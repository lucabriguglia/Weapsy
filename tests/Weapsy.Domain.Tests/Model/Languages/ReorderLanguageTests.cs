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
    public class ReorderLanguageTests
    {
        private Language language;
        private Guid languageId;
        private int newSortOrder;
        private LanguageReordered @event;

        [SetUp]
        public void Setup()
        {
            languageId = Guid.NewGuid();
            newSortOrder = 1;

            var command = new CreateLanguage
            {
                SiteId = Guid.NewGuid(),
                Id = languageId,
                Name = "My Language",
                CultureName = "aa-bb",
                Url = "url"
            };

            var validatorMock = new Mock<IValidator<CreateLanguage>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            var sortOrderGeneratorMock = new Mock<ILanguageSortOrderGenerator>();
            sortOrderGeneratorMock.Setup(x => x.GenerateNextSortOrder(command.SiteId)).Returns(2);

            language = Language.CreateNew(command, validatorMock.Object, sortOrderGeneratorMock.Object);
            
            language.Reorder(newSortOrder);

            @event = language.Events.OfType<LanguageReordered>().SingleOrDefault();
        }

        [Test]
        public void Should_set_sort_order()
        {
            Assert.AreEqual(newSortOrder, language.SortOrder);
        }

        [Test]
        public void Should_add_language_reordered_event()
        {
            Assert.IsNotNull(@event);
        }

        [Test]
        public void Should_set_id_in_language_reordered_event()
        {
            Assert.AreEqual(language.Id, @event.AggregateRootId);
        }

        [Test]
        public void Should_set_site_id_in_language_reordered_event()
        {
            Assert.AreEqual(language.SiteId, @event.SiteId);
        }

        [Test]
        public void Should_set_sort_order_in_language_reordered_event()
        {
            Assert.AreEqual(language.SortOrder, @event.SortOrder);
        }

        [Test]
        public void Should_throw_exception_if_language_is_deleted()
        {
            var deleteLanguageCommand = new DeleteLanguage
            {
                SiteId = language.SiteId,
                Id = language.Id
            };
            var deleteLanguageValidatorMock = new Mock<IValidator<DeleteLanguage>>();
            deleteLanguageValidatorMock.Setup(x => x.Validate(deleteLanguageCommand)).Returns(new ValidationResult());
            language.Delete(deleteLanguageCommand, deleteLanguageValidatorMock.Object);
            Assert.Throws<Exception>(() => language.Reorder(1));
        }
    }
}
