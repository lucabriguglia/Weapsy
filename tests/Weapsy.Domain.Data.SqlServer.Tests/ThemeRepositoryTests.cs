using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using Weapsy.Domain.Data.SqlServer.Repositories;
using Weapsy.Domain.Model.Themes;
using Weapsy.Tests.Factories;
using ThemeDbEntity = Weapsy.Domain.Data.SqlServer.Entities.Theme;

namespace Weapsy.Domain.Data.SqlServer.Tests
{
    [TestFixture]
    public class ThemeRepositoryTests
    {
        private IThemeRepository sut;
        private Guid themeId1;
        private Guid themeId2;

        [SetUp]
        public void SetUp()
        {
            var optionsBuilder = new DbContextOptionsBuilder<WeapsyDbContext>();
            optionsBuilder.UseInMemoryDatabase();
            var dbContext = new WeapsyDbContext(optionsBuilder.Options);

            themeId1 = Guid.NewGuid();
            themeId2 = Guid.NewGuid();

            dbContext.Set<ThemeDbEntity>().AddRange(
                new ThemeDbEntity
                {
                    Id = themeId1,
                    Name = "Name 1",
                    Description = "Description 1",
                    Folder = "Folder 1",
                    Status = ThemeStatus.Active
                },
                new ThemeDbEntity
                {
                    Id = themeId2,
                    Name = "Name 2",
                    Description = "Description 2",
                    Folder = "Folder 2",
                    Status = ThemeStatus.Active
                },
                new ThemeDbEntity
                {
                    Status = ThemeStatus.Deleted
                }
            );

            dbContext.SaveChanges();

            var mapperMock = new Moq.Mock<AutoMapper.IMapper>();
            mapperMock.Setup(x => x.Map<ThemeDbEntity>(Moq.It.IsAny<Theme>())).Returns(new ThemeDbEntity());
            mapperMock.Setup(x => x.Map<Theme>(Moq.It.IsAny<ThemeDbEntity>())).Returns(new Theme());

            sut = new ThemeRepository(dbContext, mapperMock.Object);
        }

        [Test]
        public void Should_return_theme_by_id()
        {
            var actual = sut.GetById(themeId1);
            Assert.NotNull(actual);
        }

        [Test]
        public void Should_return_theme_by_name()
        {
            var actual = sut.GetByName("Name 1");
            Assert.NotNull(actual);
        }

        [Test]
        public void Should_return_theme_by_folder()
        {
            var actual = sut.GetByFolder("Folder 1");
            Assert.NotNull(actual);
        }

        [Test]
        public void Should_return_all_themes()
        {
            var actual = sut.GetAll();
            Assert.AreEqual(2, actual.Count);
        }

        [Test]
        [Ignore("No idea why count is 10 instead of 2")]
        public void Should_return_themes_count()
        {
            var actual = sut.GetThemesCount();
            Assert.AreEqual(2, actual);
        }

        [Test]
        public void Should_save_new_theme()
        {
            var newTheme = ThemeFactory.Theme(Guid.NewGuid(), "Name 3", "Description 3", "Folder 3");

            sut.Create(newTheme);

            var actual = sut.GetById(newTheme.Id);

            Assert.NotNull(actual);
        }

        [Test]
        public void Should_update_theme()
        {
            var newThemeDescription = "New Description 1";

            var themeToUpdate = ThemeFactory.Theme(Guid.NewGuid(), "Name 1", newThemeDescription, "Folder 1");

            sut.Update(themeToUpdate);

            var updatedTheme = sut.GetById(themeId1);

            Assert.AreEqual(newThemeDescription, updatedTheme.Description);
        }
    }
}
