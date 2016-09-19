using System;
using NUnit.Framework;
using Moq;
using Weapsy.Domain.Languages;

namespace Weapsy.Domain.Tests.Languages
{
    [TestFixture]
    public class LanguageSortOrderGeneratorTests
    {
        [Test]
        public void Should_add_one_to_active_languages_count_when_generate_new_sort_order()
        {
            var siteId = Guid.NewGuid();

            var languageRepositoryMock = new Mock<ILanguageRepository>();
            languageRepositoryMock.Setup(x => x.GetLanguagesCount(siteId)).Returns(4);

            var sut = new LanguageSortOrderGenerator(languageRepositoryMock.Object);

            var actual = sut.GenerateNextSortOrder(siteId);

            Assert.AreEqual(5, actual);
        }
    }
}
