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
    public class ReorderLanguageTests
    {
        private Language _language;
        private Guid _languageId;
        private int _newSortOrder;
        private LanguageReordered _event;

        [SetUp]
        public void Setup()
        {
            _languageId = Guid.NewGuid();
            _newSortOrder = 1;

            var command = new CreateLanguage
            {
                SiteId = Guid.NewGuid(),
                Id = _languageId,
                Name = "My Language",
                CultureName = "aa-bb",
                Url = "url"
            };

            var validatorMock = new Mock<IValidator<CreateLanguage>>();
            validatorMock.Setup(x => x.Validate(command)).Returns(new ValidationResult());

            var sortOrderGeneratorMock = new Mock<ILanguageSortOrderGenerator>();
            sortOrderGeneratorMock.Setup(x => x.GenerateNextSortOrder(command.SiteId)).Returns(2);

            _language = Language.CreateNew(command, validatorMock.Object, sortOrderGeneratorMock.Object);
            
            _language.Reorder(_newSortOrder);

            _event = _language.Events.OfType<LanguageReordered>().SingleOrDefault();
        }

        [Test]
        public void Should_set_sort_order()
        {
            Assert.AreEqual(_newSortOrder, _language.SortOrder);
        }

        [Test]
        public void Should_add_language_reordered_event()
        {
            Assert.IsNotNull(_event);
        }

        [Test]
        public void Should_set_id_in_language_reordered_event()
        {
            Assert.AreEqual(_language.Id, _event.AggregateRootId);
        }

        [Test]
        public void Should_set_site_id_in_language_reordered_event()
        {
            Assert.AreEqual(_language.SiteId, _event.SiteId);
        }

        [Test]
        public void Should_set_sort_order_in_language_reordered_event()
        {
            Assert.AreEqual(_language.SortOrder, _event.SortOrder);
        }

        [Test]
        public void Should_throw_exception_if_language_is_deleted()
        {
            var deleteLanguageCommand = new DeleteLanguage
            {
                SiteId = _language.SiteId,
                Id = _language.Id
            };
            var deleteLanguageValidatorMock = new Mock<IValidator<DeleteLanguage>>();
            deleteLanguageValidatorMock.Setup(x => x.Validate(deleteLanguageCommand)).Returns(new ValidationResult());
            _language.Delete(deleteLanguageCommand, deleteLanguageValidatorMock.Object);
            Assert.Throws<Exception>(() => _language.Reorder(1));
        }
    }
}
