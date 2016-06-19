using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using Weapsy.Domain.Data.Repositories;
using Weapsy.Domain.Model.Languages;
using Weapsy.Tests.Factories;
using LanguageDbEntity = Weapsy.Domain.Data.Entities.Language;

namespace Weapsy.Domain.Data.SqlServer.Tests
{
    [TestFixture]
    public class LanguageRepositoryTests
    {
        private ILanguageRepository sut;
        private Guid siteId;
        private Guid languageId1;
        private Guid languageId2;

        [SetUp]
        public void SetUp()
        {
            var optionsBuilder = new DbContextOptionsBuilder<WeapsyDbContext>();
            optionsBuilder.UseInMemoryDatabase();
            var dbContext = new WeapsyDbContext(optionsBuilder.Options);

            siteId = Guid.NewGuid();
            languageId1 = Guid.NewGuid();
            languageId2 = Guid.NewGuid();

            dbContext.Set<LanguageDbEntity>().AddRange(
                new LanguageDbEntity
                {
                    SiteId = siteId,
                    Id = languageId1,
                    Name = "Language Name 1",
                    CultureName = "ab1",
                    Url = "ab1",
                    Status = LanguageStatus.Active
                },
                new LanguageDbEntity
                {
                    SiteId = siteId,
                    Id = languageId2,
                    Name = "Language Name 2",
                    CultureName = "ab2",
                    Url = "ab2",
                    Status = LanguageStatus.Hidden
                },
                new LanguageDbEntity
                {
                    Status = LanguageStatus.Deleted
                }
            );

            dbContext.SaveChanges();

            var mapperMock = new Moq.Mock<AutoMapper.IMapper>();
            mapperMock.Setup(x => x.Map<LanguageDbEntity>(Moq.It.IsAny<Language>())).Returns(new LanguageDbEntity());
            mapperMock.Setup(x => x.Map<Language>(Moq.It.IsAny<LanguageDbEntity>())).Returns(new Language());

            sut = new LanguageRepository(dbContext, mapperMock.Object);
        }

        [Test]
        public void Should_return_language_by_id_only()
        {
            var actual = sut.GetById(languageId1);
            Assert.NotNull(actual);
        }

        [Test]
        public void Should_return_language_by_id()
        {
            var actual = sut.GetById(siteId, languageId1);
            Assert.NotNull(actual);
        }

        [Test]
        public void Should_return_language_by_name()
        {
            var actual = sut.GetByName(siteId, "Language Name 1");
            Assert.NotNull(actual);
        }

        [Test]
        public void Should_return_language_by_culture_name()
        {
            var actual = sut.GetByCultureName(siteId, "ab1");
            Assert.NotNull(actual);
        }

        [Test]
        public void Should_return_language_by_url()
        {
            var actual = sut.GetByUrl(siteId, "ab1");
            Assert.NotNull(actual);
        }

        [Test]
        public void Should_return_languages_count()
        {
            var actual = sut.GetLanguagesCount(siteId);
            Assert.AreEqual(2, actual);
        }

        [Test]
        public void Should_return_active_languages_count()
        {
            var actual = sut.GetActiveLanguagesCount(siteId);
            Assert.AreEqual(1, actual);
        }

        [Test]
        public void Should_return_languages_id_list()
        {
            var actual = sut.GetLanguagesIdList(siteId);
            Assert.AreEqual(new List<Guid> { languageId1, languageId2 }, actual);
        }

        [Test]
        public void Should_return_all_languages()
        {
            var actual = sut.GetAll(siteId);
            Assert.AreEqual(2, actual.Count);
        }

        [Test]
        public void Should_save_new_language()
        {
            var newLanguage = LanguageFactory.Language();

            sut.Create(newLanguage);

            var actual = sut.GetById(siteId, newLanguage.Id);

            Assert.NotNull(actual);
        }

        [Test]
        public void Should_update_language()
        {
            var newLanguageName = "New Language Name 1";

            var languageToUpdate = LanguageFactory.Language(siteId, languageId1, newLanguageName, "en", "en");

            sut.Update(languageToUpdate);

            var updatedLanguage = sut.GetById(siteId, languageId1);

            Assert.AreEqual(newLanguageName, updatedLanguage.Name);
        }

        [Test]
        public void Should_update_languages()
        {
            var newLanguageName1 = "New Language Name 1";
            var newLanguageName2 = "New Language Name 2";

            var languageToUpdate1 = LanguageFactory.Language(siteId, languageId1, newLanguageName1, "ab1", "ab1");
            var languageToUpdate2 = LanguageFactory.Language(siteId, languageId2, newLanguageName2, "ab2", "ab2");

            sut.Update(new List<Language> { languageToUpdate1, languageToUpdate2 });

            var updatedLanguage1 = sut.GetById(siteId, languageId1);
            var updatedLanguage2 = sut.GetById(siteId, languageId2);

            Assert.AreEqual(newLanguageName1, updatedLanguage1.Name);
            Assert.AreEqual(newLanguageName2, updatedLanguage2.Name);
        }
    }
}
