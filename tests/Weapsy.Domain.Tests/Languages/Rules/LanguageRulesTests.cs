using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Weapsy.Domain.Languages;
using Weapsy.Domain.Languages.Rules;

namespace Weapsy.Domain.Tests.Languages.Rules
{
    [TestFixture]
    public class LanguageRulesTests
    {
        [Test]
        public void Should_return_false_if_language_id_is_not_unique()
        {
            var id = Guid.NewGuid();

            var repositoryMock = new Mock<ILanguageRepository>();
            repositoryMock.Setup(x => x.GetById(id)).Returns(new Language());

            var sut = new LanguageRules(repositoryMock.Object);

            var actual = sut.IsLanguageIdUnique(id);

            Assert.AreEqual(false, actual);
        }

        [Test]
        public void Should_return_true_if_language_id_is_unique()
        {
            var id = Guid.NewGuid();

            var repositoryMock = new Mock<ILanguageRepository>();
            repositoryMock.Setup(x => x.GetById(id)).Returns((Language)null);

            var sut = new LanguageRules(repositoryMock.Object);

            var actual = sut.IsLanguageIdUnique(id);

            Assert.AreEqual(true, actual);
        }

        [TestCase("a@b")]
        [TestCase("a!b")]
        [TestCase("")]
        public void Should_return_false_if_language_name_is_not_valid(string name)
        {
            var sut = new LanguageRules(new Mock<ILanguageRepository>().Object);
            var actual = sut.IsLanguageNameValid(name);
            Assert.AreEqual(false, actual);
        }

        [TestCase("ab")]
        [TestCase("a-b")]
        [TestCase("a_b")]
        [TestCase("a1-b2")]
        [TestCase("a b")]
        public void Should_return_true_if_language_name_is_valid(string name)
        {
            var sut = new LanguageRules(new Mock<ILanguageRepository>().Object);
            var actual = sut.IsLanguageNameValid(name);
            Assert.AreEqual(true, actual);
        }

        [Test]
        public void Should_return_false_if_language_name_is_not_unique()
        {
            var siteId = Guid.NewGuid();
            var name = "My Language";

            var repositoryMock = new Mock<ILanguageRepository>();
            repositoryMock.Setup(x => x.GetByName(siteId, name)).Returns(new Language());

            var sut = new LanguageRules(repositoryMock.Object);

            var actual = sut.IsLanguageNameUnique(siteId, name);

            Assert.AreEqual(false, actual);
        }

        [Test]
        public void Should_return_true_if_language_name_is_unique()
        {
            var siteId = Guid.NewGuid();
            var name = "My Language";

            var repositoryMock = new Mock<ILanguageRepository>();
            repositoryMock.Setup(x => x.GetByName(siteId, name)).Returns((Language)null);

            var sut = new LanguageRules(repositoryMock.Object);

            var actual = sut.IsLanguageNameUnique(siteId, name);

            Assert.AreEqual(true, actual);
        }

        [TestCase("a@b")]
        [TestCase("a b")]
        [TestCase("aa-bbc")]
        [TestCase("")]
        public void Should_return_false_if_culture_name_is_not_valid(string cultureName)
        {
            var sut = new LanguageRules(new Mock<ILanguageRepository>().Object);
            var actual = sut.IsCultureNameValid(cultureName);
            Assert.AreEqual(false, actual);
        }

        [TestCase("es")]
        [TestCase("en-GB")]
        public void Should_return_true_if_culture_name_is_valid(string cultureName)
        {
            var sut = new LanguageRules(new Mock<ILanguageRepository>().Object);
            var actual = sut.IsCultureNameValid(cultureName);
            Assert.AreEqual(true, actual);
        }

        [Test]
        public void Should_return_false_if_culture_name_is_not_unique()
        {
            var siteId = Guid.NewGuid();
            var cultureName = "aa-bb";

            var repositoryMock = new Mock<ILanguageRepository>();
            repositoryMock.Setup(x => x.GetByCultureName(siteId, cultureName)).Returns(new Language());

            var sut = new LanguageRules(repositoryMock.Object);

            var actual = sut.IsCultureNameUnique(siteId, cultureName);

            Assert.AreEqual(false, actual);
        }

        [Test]
        public void Should_return_true_if_culture_name_is_unique()
        {
            var siteId = Guid.NewGuid();
            var cultureName = "aa-bb";

            var repositoryMock = new Mock<ILanguageRepository>();
            repositoryMock.Setup(x => x.GetByCultureName(siteId, cultureName)).Returns((Language)null);

            var sut = new LanguageRules(repositoryMock.Object);

            var actual = sut.IsCultureNameUnique(siteId, cultureName);

            Assert.AreEqual(true, actual);
        }

        [TestCase("a@b")]
        [TestCase("a!b")]
        [TestCase("a b")]
        [TestCase("")]
        public void Should_return_false_if_language_url_is_not_valid(string url)
        {
            var sut = new LanguageRules(new Mock<ILanguageRepository>().Object);
            var actual = sut.IsLanguageUrlValid(url);
            Assert.AreEqual(false, actual);
        }

        [TestCase("ab")]
        [TestCase("a-b")]
        [TestCase("a_b")]
        [TestCase("a1-b2")]
        public void Should_return_true_if_language_url_is_valid(string url)
        {
            var sut = new LanguageRules(new Mock<ILanguageRepository>().Object);
            var actual = sut.IsLanguageUrlValid(url);
            Assert.AreEqual(true, actual);
        }

        [Test]
        public void Should_return_false_if_language_url_is_not_unique()
        {
            var siteId = Guid.NewGuid();
            var url = "my-language";

            var repositoryMock = new Mock<ILanguageRepository>();
            repositoryMock.Setup(x => x.GetByUrl(siteId, url)).Returns(new Language());

            var sut = new LanguageRules(repositoryMock.Object);

            var actual = sut.IsLanguageUrlUnique(siteId, url);

            Assert.AreEqual(false, actual);
        }

        [Test]
        public void Should_return_true_if_language_url_is_unique()
        {
            var siteId = Guid.NewGuid();
            var url = "my-language";

            var repositoryMock = new Mock<ILanguageRepository>();
            repositoryMock.Setup(x => x.GetByUrl(siteId, url)).Returns((Language)null);

            var sut = new LanguageRules(repositoryMock.Object);

            var actual = sut.IsLanguageUrlUnique(siteId, url);

            Assert.AreEqual(true, actual);
        }

        [Test]
        public void Should_return_false_if_not_all_languages_are_included()
        {
            var siteId = Guid.NewGuid();
            var languageId1 = Guid.NewGuid();
            var languageId2 = Guid.NewGuid();
            var ids = new List<Guid> { languageId1 };

            var repositoryMock = new Mock<ILanguageRepository>();
            repositoryMock.Setup(x => x.GetLanguagesIdList(siteId)).Returns(new List<Guid> { languageId1, languageId2 });

            var sut = new LanguageRules(repositoryMock.Object);

            var actual = sut.AreAllSupportedLanguagesIncluded(siteId, ids);

            Assert.AreEqual(false, actual);
        }

        [Test]
        public void Should_return_true_if_all_languages_are_included()
        {
            var siteId = Guid.NewGuid();
            var languageId1 = Guid.NewGuid();
            var languageId2 = Guid.NewGuid();
            var ids = new List<Guid> { languageId1, languageId2 };

            var repositoryMock = new Mock<ILanguageRepository>();
            repositoryMock.Setup(x => x.GetLanguagesIdList(siteId)).Returns(new List<Guid> { languageId1, languageId2 });

            var sut = new LanguageRules(repositoryMock.Object);

            var actual = sut.AreAllSupportedLanguagesIncluded(siteId, ids);

            Assert.AreEqual(true, actual);
        }
    }
}
