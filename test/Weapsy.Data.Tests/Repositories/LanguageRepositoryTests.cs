using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Weapsy.Data.Domain;
using Weapsy.Domain.Languages;
using Weapsy.Tests.Factories;
using LanguageDbEntity = Weapsy.Data.Entities.Language;

namespace Weapsy.Data.Tests.Repositories
{
    [TestFixture]
    public class LanguageRepositoryTests
    {
        private DbContextOptions<WeapsyDbContext> _contextOptions;
        private Guid _siteId;
        private Guid _languageId1;
        private Guid _languageId2;
        private Guid _deletedLanguageId;

        [SetUp]
        public void SetUp()
        {
            _contextOptions = DbContextShared.CreateContextOptions();

            using (var context = new WeapsyDbContext(_contextOptions))
            {
                _siteId = Guid.NewGuid();
                _languageId1 = Guid.NewGuid();
                _languageId2 = Guid.NewGuid();
                _deletedLanguageId = Guid.NewGuid();

                context.Languages.AddRange(
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
                        Id = _deletedLanguageId,
                        Status = LanguageStatus.Deleted
                    }
                );

                context.SaveChanges();                
            }
        }

        [Test]
        public void Should_return_null_if_language_is_deleted()
        {
            using (var context = new WeapsyDbContext(_contextOptions))
            {
                var repository = new LanguageRepository(DbContextShared.CreateNewContextFactory(context), Shared.CreateNewMapper());
                var language = repository.GetById(_deletedLanguageId);

                Assert.Null(language);
            }
        }

        [Test]
        public void Should_return_language_by_id_only()
        {
            using (var context = new WeapsyDbContext(_contextOptions))
            {
                var repository = new LanguageRepository(DbContextShared.CreateNewContextFactory(context), Shared.CreateNewMapper());
                var language = repository.GetById(_languageId1);

                Assert.NotNull(language);
            }
        }

        [Test]
        public void Should_return_language_by_id()
        {
            using (var context = new WeapsyDbContext(_contextOptions))
            {
                var repository = new LanguageRepository(DbContextShared.CreateNewContextFactory(context), Shared.CreateNewMapper());
                var language = repository.GetById(_siteId, _languageId1);

                Assert.NotNull(language);
            }
        }

        [Test]
        public void Should_return_language_by_name()
        {
            using (var context = new WeapsyDbContext(_contextOptions))
            {
                var repository = new LanguageRepository(DbContextShared.CreateNewContextFactory(context), Shared.CreateNewMapper());
                var language = repository.GetByName(_siteId, "Language Name 1");

                Assert.NotNull(language);
            }
        }

        [Test]
        public void Should_return_language_by_culture_name()
        {
            using (var context = new WeapsyDbContext(_contextOptions))
            {
                var repository = new LanguageRepository(DbContextShared.CreateNewContextFactory(context), Shared.CreateNewMapper());
                var language = repository.GetByCultureName(_siteId, "ab1");

                Assert.NotNull(language);
            }
        }

        [Test]
        public void Should_return_language_by_url()
        {
            using (var context = new WeapsyDbContext(_contextOptions))
            {
                var repository = new LanguageRepository(DbContextShared.CreateNewContextFactory(context), Shared.CreateNewMapper());
                var language = repository.GetByUrl(_siteId, "ab1");

                Assert.NotNull(language);
            }
        }

        [Test]
        public void Should_return_languages_count()
        {
            using (var context = new WeapsyDbContext(_contextOptions))
            {
                var repository = new LanguageRepository(DbContextShared.CreateNewContextFactory(context), Shared.CreateNewMapper());
                var count = repository.GetLanguagesCount(_siteId);

                Assert.AreEqual(2, count);
            }
        }

        [Test]
        public void Should_return_languages_id_list()
        {
            using (var context = new WeapsyDbContext(_contextOptions))
            {
                var repository = new LanguageRepository(DbContextShared.CreateNewContextFactory(context), Shared.CreateNewMapper());
                var list = repository.GetLanguagesIdList(_siteId);

                Assert.AreEqual(new List<Guid> { _languageId1, _languageId2 }, list);
            }
        }

        [Test]
        public void Should_return_all_languages()
        {
            using (var context = new WeapsyDbContext(_contextOptions))
            {
                var repository = new LanguageRepository(DbContextShared.CreateNewContextFactory(context), Shared.CreateNewMapper());
                var list = repository.GetAll(_siteId);

                Assert.AreEqual(2, list.Count);
            }
        }

        [Test]
        public void Should_save_new_language()
        {
            var newLanguage = LanguageFactory.Language(_siteId, Guid.NewGuid(), "Name", "CultureName", "Url");

            using (var context = new WeapsyDbContext(_contextOptions))
            {
                var repository = new LanguageRepository(DbContextShared.CreateNewContextFactory(context), Shared.CreateNewMapper());
                repository.Create(newLanguage);
            }

            using (var context = new WeapsyDbContext(_contextOptions))
            {
                var repository = new LanguageRepository(DbContextShared.CreateNewContextFactory(context), Shared.CreateNewMapper());
                var language = repository.GetById(_siteId, newLanguage.Id);

                Assert.NotNull(language);
            }
        }

        [Test]
        public void Should_update_language()
        {
            const string newLanguageName = "New Language Name 1";

            var languageToUpdate = LanguageFactory.Language(_siteId, _languageId1, newLanguageName, "en", "en");

            using (var context = new WeapsyDbContext(_contextOptions))
            {
                var repository = new LanguageRepository(DbContextShared.CreateNewContextFactory(context), Shared.CreateNewMapper());
                repository.Update(languageToUpdate);
            }

            using (var context = new WeapsyDbContext(_contextOptions))
            {
                var repository = new LanguageRepository(DbContextShared.CreateNewContextFactory(context), Shared.CreateNewMapper());
                var updatedLanguage = repository.GetById(_siteId, _languageId1);

                Assert.AreEqual(newLanguageName, updatedLanguage.Name);
            }
        }

        [Test]
        public void Should_update_languages()
        {
            var newLanguageName1 = "New Language Name 1";
            var newLanguageName2 = "New Language Name 2";

            var languageToUpdate1 = LanguageFactory.Language(_siteId, _languageId1, newLanguageName1, "ab1", "ab1");
            var languageToUpdate2 = LanguageFactory.Language(_siteId, _languageId2, newLanguageName2, "ab2", "ab2");

            using (var context = new WeapsyDbContext(_contextOptions))
            {
                var repository = new LanguageRepository(DbContextShared.CreateNewContextFactory(context), Shared.CreateNewMapper());
                repository.Update(new List<Language> { languageToUpdate1, languageToUpdate2 });
            }

            using (var context = new WeapsyDbContext(_contextOptions))
            {
                var repository = new LanguageRepository(DbContextShared.CreateNewContextFactory(context), Shared.CreateNewMapper());
                var updatedLanguage1 = repository.GetById(_siteId, _languageId1);

                Assert.AreEqual(newLanguageName1, updatedLanguage1.Name);
            }

            using (var context = new WeapsyDbContext(_contextOptions))
            {
                var repository = new LanguageRepository(DbContextShared.CreateNewContextFactory(context), Shared.CreateNewMapper());
                var updatedLanguage2 = repository.GetById(_siteId, _languageId2);

                Assert.AreEqual(newLanguageName2, updatedLanguage2.Name);
            }
        }
    }
}
