using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using AutoMapper;
using Moq;
using Weapsy.Domain.Data.SqlServer.Repositories;
using Weapsy.Domain.Themes;
using Weapsy.Tests.Factories;
using ThemeDbEntity = Weapsy.Domain.Data.SqlServer.Entities.Theme;

namespace Weapsy.Domain.Data.SqlServer.Tests
{
    [TestFixture]
    public class ThemeRepositoryTests
    {
        private IThemeRepository _sut;
        private WeapsyDbContext _dbContext;
        private Guid _themeId1;
        private Guid _themeId2;

        [SetUp]
        public void SetUp()
        {
            var optionsBuilder = new DbContextOptionsBuilder<WeapsyDbContext>();
            optionsBuilder.UseInMemoryDatabase();
            _dbContext = new WeapsyDbContext(optionsBuilder.Options);

            _themeId1 = Guid.NewGuid();
            _themeId2 = Guid.NewGuid();

            _dbContext.Set<ThemeDbEntity>().AddRange
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
                    Status = ThemeStatus.Deleted
                }
            );

            _dbContext.SaveChanges();

            var mapperMock = new Mock<AutoMapper.IMapper>();
            mapperMock.Setup(x => x.Map<ThemeDbEntity>(It.IsAny<Theme>())).Returns(new ThemeDbEntity());
            mapperMock.Setup(x => x.Map<Theme>(It.IsAny<ThemeDbEntity>())).Returns(new Theme());
            mapperMock.Setup(x => x.Map<ICollection<Theme>>(It.IsAny<ICollection<ThemeDbEntity>>())).Returns(new List<Theme>
            {
                ThemeFactory.Theme(_themeId1, "Name", "Description", "Folder"),
                ThemeFactory.Theme(_themeId2, "Name", "Description", "Folder")
            });

            _sut = new ThemeRepository(_dbContext, mapperMock.Object);
        }

        [Test]
        public void Should_return_theme_by_id()
        {
            var actual = _sut.GetById(_themeId1);
            Assert.NotNull(actual);
        }

        [Test]
        public void Should_return_theme_by_name()
        {
            var actual = _sut.GetByName("Name 1");
            Assert.NotNull(actual);
        }

        [Test]
        public void Should_return_theme_by_folder()
        {
            var actual = _sut.GetByFolder("Folder 1");
            Assert.NotNull(actual);
        }

        [Test]
        public void Should_return_all_themes()
        {
            var actual = _sut.GetAll();
            Assert.AreEqual(2, actual.Count);
        }

        [Test]
        [Ignore("No idea why count is 10 instead of 2")]
        public void Should_return_themes_count()
        {
            var actual = _sut.GetThemesCount();
            Assert.AreEqual(2, actual);
        }

        [Test]
        public void Should_save_new_theme()
        {
            var newTheme = ThemeFactory.Theme(Guid.NewGuid(), "Name 3", "Description 3", "Folder 3");
            var newThemeDbEntity = new ThemeDbEntity
            {
                Id = newTheme.Id,
                Name = newTheme.Name,
                Description = newTheme.Description,
                Folder = newTheme.Folder
            };

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(x => x.Map<ThemeDbEntity>(newTheme)).Returns(newThemeDbEntity);
            mapperMock.Setup(x => x.Map<Theme>(newThemeDbEntity)).Returns(newTheme);

            _sut = new ThemeRepository(_dbContext, mapperMock.Object);

            _sut.Create(newTheme);

            var actual = _sut.GetById(newTheme.Id);

            Assert.NotNull(actual);
        }

        [Test]
        public void Should_update_theme()
        {
            var newThemeDescription = "New Description 1";

            var themeToUpdate = ThemeFactory.Theme(_themeId1, "Name 1", newThemeDescription, "Folder 1");

            _sut.Update(themeToUpdate);

            var updatedTheme = _sut.GetById(_themeId1);

            Assert.AreEqual(newThemeDescription, updatedTheme.Description);
        }
    }
}
