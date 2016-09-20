using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using Weapsy.Domain.Data.SqlServer.Repositories;
using Weapsy.Domain.Languages;
using Weapsy.Tests.Factories;
using LanguageDbEntity = Weapsy.Domain.Data.SqlServer.Entities.Language;

namespace Weapsy.Domain.Data.SqlServer.Tests
{
    [TestFixture]
    public class LanguageRepositoryTests
    {
        private ILanguageRepository _sut;
        private Guid _siteId;
        private Guid _languageId1;
        private Guid _languageId2;

        [SetUp]
        public void SetUp()
        {
            var optionsBuilder = new DbContextOptionsBuilder<WeapsyDbContext>();
            optionsBuilder.UseInMemoryDatabase();
            var dbContext = new WeapsyDbContext(optionsBuilder.Options);

            _siteId = Guid.NewGuid();
            _languageId1 = Guid.NewGuid();
            _languageId2 = Guid.NewGuid();

            dbContext.Set<LanguageDbEntity>().AddRange(
                new LanguageDbEntity
                {
                    SiteId = _siteId,
                    Id = _languageId1,
                    Name = "Language Name 1",
                    CultureName = "ab1",
                    Url = "ab1",
                    Status = LanguageStatus.Active
                },
                new LanguageDbEntity
                {
                    SiteId = _siteId,
                    Id = _languageId2,
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
            mapperMock.Setup(x => x.Map<Language, LanguageDbEntity>(Moq.It.IsAny<Language>())).Returns(new LanguageDbEntity());
            mapperMock.Setup(x => x.Map<LanguageDbEntity, Language>(Moq.It.IsAny<LanguageDbEntity>())).Returns(new Language());

            _sut = new LanguageRepository(dbContext, mapperMock.Object);
        }

        [Test]
        public void Should_return_language_by_id_only()
        {
            var actual = _sut.GetById(_languageId1);
            Assert.NotNull(actual);
        }

        [Test]
        public void Should_return_language_by_id()
        {
            var actual = _sut.GetById(_siteId, _languageId1);
            Assert.NotNull(actual);
        }

        [Test]
        public void Should_return_language_by_name()
        {
            var actual = _sut.GetByName(_siteId, "Language Name 1");
            Assert.NotNull(actual);
        }

        [Test]
        public void Should_return_language_by_culture_name()
        {
            var actual = _sut.GetByCultureName(_siteId, "ab1");
            Assert.NotNull(actual);
        }

        [Test]
        public void Should_return_language_by_url()
        {
            var actual = _sut.GetByUrl(_siteId, "ab1");
            Assert.NotNull(actual);
        }

        [Test]
        public void Should_return_languages_count()
        {
            var actual = _sut.GetLanguagesCount(_siteId);
            Assert.AreEqual(2, actual);
        }

        [Test]
        public void Should_return_active_languages_count()
        {
            var actual = _sut.GetActiveLanguagesCount(_siteId);
            Assert.AreEqual(1, actual);
        }

        [Test]
        public void Should_return_languages_id_list()
        {
            var actual = _sut.GetLanguagesIdList(_siteId);
            Assert.AreEqual(new List<Guid> { _languageId1, _languageId2 }, actual);
        }

        [Test]
        public void Should_return_all_languages()
        {
            var actual = _sut.GetAll(_siteId);
            Assert.AreEqual(2, actual.Count);
        }

        [Test]
        public void Should_save_new_language()
        {
            var newLanguage = LanguageFactory.Language();

            _sut.Create(newLanguage);

            var actual = _sut.GetById(_siteId, newLanguage.Id);

            Assert.NotNull(actual);
        }

        [Test]
        public void Should_update_language()
        {
            var newLanguageName = "New Language Name 1";

            var languageToUpdate = LanguageFactory.Language(_siteId, _languageId1, newLanguageName, "en", "en");

            _sut.Update(languageToUpdate);

            var updatedLanguage = _sut.GetById(_siteId, _languageId1);

            Assert.AreEqual(newLanguageName, updatedLanguage.Name);
        }

        [Test]
        public void Should_update_languages()
        {
            var newLanguageName1 = "New Language Name 1";
            var newLanguageName2 = "New Language Name 2";

            var languageToUpdate1 = LanguageFactory.Language(_siteId, _languageId1, newLanguageName1, "ab1", "ab1");
            var languageToUpdate2 = LanguageFactory.Language(_siteId, _languageId2, newLanguageName2, "ab2", "ab2");

            _sut.Update(new List<Language> { languageToUpdate1, languageToUpdate2 });

            var updatedLanguage1 = _sut.GetById(_siteId, _languageId1);
            var updatedLanguage2 = _sut.GetById(_siteId, _languageId2);

            Assert.AreEqual(newLanguageName1, updatedLanguage1.Name);
            Assert.AreEqual(newLanguageName2, updatedLanguage2.Name);
        }
    }
}
