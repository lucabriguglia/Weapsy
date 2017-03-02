using System;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Weapsy.Data.Domain;
using Weapsy.Domain.Themes;
using Weapsy.Tests.Factories;
using ThemeDbEntity = Weapsy.Data.Entities.Theme;

namespace Weapsy.Data.Tests.Repositories
{
    [TestFixture]
    public class ThemeRepositoryTests
    {
        private DbContextOptions<WeapsyDbContext> _contextOptions;
        private Guid _themeId1;
        private Guid _themeId2;
        private Guid _deletedThemeId;

        [SetUp]
        public void SetUp()
        {
            _contextOptions = DbContextShared.CreateContextOptions();

            using (var context = new WeapsyDbContext(_contextOptions))
            {
                _themeId1 = Guid.NewGuid();
                _themeId2 = Guid.NewGuid();
                _deletedThemeId = Guid.NewGuid();

                context.Themes.AddRange
                (
                    new ThemeDbEntity
                    {
                        Id = _themeId1,
                        Name = "Name 1",
                        Description = "Description 1",
                        Folder = "Folder 1",
                        Status = ThemeStatus.Active
                    },
                    new ThemeDbEntity
                    {
                        Id = _themeId2,
                        Name = "Name 2",
                        Description = "Description 2",
                        Folder = "Folder 2",
                        Status = ThemeStatus.Active
                    },
                    new ThemeDbEntity
                    {
                        Id = _deletedThemeId,
                        Status = ThemeStatus.Deleted
                    }
                );

                context.SaveChanges();
            }
        }

        [Test]
        public void Should_return_null_if_theme_is_deleted()
        {
            using (var context = new WeapsyDbContext(_contextOptions))
            {
                var repository = new ThemeRepository(DbContextShared.CreateNewContextFactory(context), Shared.CreateNewMapper());
                var theme = repository.GetById(_deletedThemeId);

                Assert.Null(theme);
            }
        }

        [Test]
        public void Should_return_theme_by_id()
        {
            using (var context = new WeapsyDbContext(_contextOptions))
            {
                var repository = new ThemeRepository(DbContextShared.CreateNewContextFactory(context), Shared.CreateNewMapper());
                var theme = repository.GetById(_themeId1);

                Assert.NotNull(theme);
            }
        }

        [Test]
        public void Should_return_theme_by_name()
        {
            using (var context = new WeapsyDbContext(_contextOptions))
            {
                var repository = new ThemeRepository(DbContextShared.CreateNewContextFactory(context), Shared.CreateNewMapper());
                var theme = repository.GetByName("Name 1");

                Assert.NotNull(theme);
            }
        }

        [Test]
        public void Should_return_theme_by_folder()
        {
            using (var context = new WeapsyDbContext(_contextOptions))
            {
                var repository = new ThemeRepository(DbContextShared.CreateNewContextFactory(context), Shared.CreateNewMapper());
                var theme = repository.GetByFolder("Folder 1");

                Assert.NotNull(theme);
            }
        }

        [Test]
        public void Should_return_themes_count()
        {
            using (var context = new WeapsyDbContext(_contextOptions))
            {
                var repository = new ThemeRepository(DbContextShared.CreateNewContextFactory(context), Shared.CreateNewMapper());
                var count = repository.GetThemesCount();

                Assert.AreEqual(2, count);
            }
        }

        [Test]
        public void Should_save_new_theme()
        {
            var newTheme = ThemeFactory.Theme(Guid.NewGuid(), "Name 3", "Description 3", "Folder 3");

            using (var context = new WeapsyDbContext(_contextOptions))
            {
                var repository = new ThemeRepository(DbContextShared.CreateNewContextFactory(context), Shared.CreateNewMapper());
                repository.Create(newTheme);
            }

            using (var context = new WeapsyDbContext(_contextOptions))
            {
                var repository = new ThemeRepository(DbContextShared.CreateNewContextFactory(context), Shared.CreateNewMapper());
                var theme = repository.GetById(newTheme.Id);

                Assert.NotNull(theme);
            }
        }

        [Test]
        public void Should_update_theme()
        {
            const string newThemeDescription = "New Description 1";

            var themeToUpdate = ThemeFactory.Theme(_themeId1, "Name 1", newThemeDescription, "Folder 1");

            using (var context = new WeapsyDbContext(_contextOptions))
            {
                var repository = new ThemeRepository(DbContextShared.CreateNewContextFactory(context), Shared.CreateNewMapper());
                repository.Update(themeToUpdate);
            }

            using (var context = new WeapsyDbContext(_contextOptions))
            {
                var repository = new ThemeRepository(DbContextShared.CreateNewContextFactory(context), Shared.CreateNewMapper());
                var updatedTheme = repository.GetById(_themeId1);

                Assert.AreEqual(newThemeDescription, updatedTheme.Description);
            }
        }
    }
}
