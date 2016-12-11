using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Weapsy.Domain.Languages;
using Weapsy.Domain.Languages.Commands;
using Weapsy.Domain.Languages.Handlers;
using Weapsy.Domain.Sites.Rules;
using System.Reflection;

namespace Weapsy.Domain.Tests.Languages.Handlers
{
    [TestFixture]
    public class ReorderLanguagesHandlerTests
    {
        [Test]
        public void Should_change_sort_order_and_update_languages()
        {
            var siteId = Guid.NewGuid();
            var language1Id = Guid.NewGuid();
            var language2Id = Guid.NewGuid();

            var command = new ReorderLanguages
            {
                SiteId = siteId,
                Languages = new List<Guid>
                {
                    language1Id,
                    language2Id
                }
            };

            var language1 = new Language();
            typeof(Language).GetTypeInfo().GetProperty("Id").SetValue(language1, language1Id, null);
            typeof(Language).GetTypeInfo().GetProperty("SortOrder").SetValue(language1, 2, null);

            var language2 = new Language();
            typeof(Language).GetTypeInfo().GetProperty("Id").SetValue(language2, language2Id, null);
            typeof(Language).GetTypeInfo().GetProperty("SortOrder").SetValue(language2, 1, null);

            var languageRepositoryMock = new Mock<ILanguageRepository>();
            languageRepositoryMock.Setup(x => x.GetById(siteId, language1Id)).Returns(language1);
            languageRepositoryMock.Setup(x => x.GetById(siteId, language2Id)).Returns(language2);
            languageRepositoryMock.Setup(x => x.Update(It.IsAny<List<Language>>()));

            var siteRulesMock = new Mock<ISiteRules>();
            siteRulesMock.Setup(x => x.DoesSiteExist(command.SiteId)).Returns(true);

            var reorderLanguagesHandler = new ReorderLanguagesHandler(languageRepositoryMock.Object, siteRulesMock.Object);
            reorderLanguagesHandler.Handle(command);

            Assert.AreEqual(1, language1.SortOrder);
            Assert.AreEqual(2, language2.SortOrder);
            languageRepositoryMock.Verify(x => x.Update(It.IsAny<List<Language>>()));
        }

        [Test]
        public void Should_throw_exception_if_language_not_found()
        {
            var siteId = Guid.NewGuid();
            var languageId = Guid.NewGuid();

            var command = new ReorderLanguages
            {
                SiteId = siteId,
                Languages = new List<Guid>
                {
                    languageId
                }
            };

            var languageRepositoryMock = new Mock<ILanguageRepository>();
            languageRepositoryMock.Setup(x => x.GetById(siteId, languageId)).Returns((Language)null);

            var siteRulesMock = new Mock<ISiteRules>();
            siteRulesMock.Setup(x => x.DoesSiteExist(siteId)).Returns(true);

            var reorderLanguagesHandler = new ReorderLanguagesHandler(languageRepositoryMock.Object, siteRulesMock.Object);

            Assert.Throws<Exception>(() => reorderLanguagesHandler.Handle(command));
        }

        [Test]
        public void Should_throw_exception_if_site_not_found()
        {
            var siteId = Guid.NewGuid();
            var languageId = Guid.NewGuid();

            var command = new ReorderLanguages
            {
                SiteId = siteId,
                Languages = new List<Guid>
                {
                    languageId
                }
            };

            var languageRepositoryMock = new Mock<ILanguageRepository>();
            languageRepositoryMock.Setup(x => x.GetById(siteId, languageId)).Returns(It.IsAny<Language>());

            var siteRulesMock = new Mock<ISiteRules>();
            siteRulesMock.Setup(x => x.DoesSiteExist(siteId)).Returns(false);

            var reorderLanguagesHandler = new ReorderLanguagesHandler(languageRepositoryMock.Object, siteRulesMock.Object);

            Assert.Throws<Exception>(() => reorderLanguagesHandler.Handle(command));
        }
    }
}
