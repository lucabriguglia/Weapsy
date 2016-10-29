using System;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Weapsy.Domain.Data.SqlServer.Repositories;
using Weapsy.Domain.Sites;
using Weapsy.Tests.Factories;
using SiteDbEntity = Weapsy.Domain.Data.SqlServer.Entities.Site;

namespace Weapsy.Domain.Data.SqlServer.Tests.Repositories
{
    [TestFixture]
    public class SiteRepositoryTests
    {
        private ISiteRepository _sut;
        private WeapsyDbContext _dbContext;
        private Guid _siteId1;
        private Guid _siteId2;

        [SetUp]
        public void SetUp()
        {
            var optionsBuilder = new DbContextOptionsBuilder<WeapsyDbContext>();
            optionsBuilder.UseInMemoryDatabase();
            _dbContext = new WeapsyDbContext(optionsBuilder.Options);

            _siteId1 = Guid.NewGuid();
            _siteId2 = Guid.NewGuid();

            _dbContext.Set<SiteDbEntity>().AddRange(
                new SiteDbEntity
                {
                    Id = _siteId1,
                    Name = "Name 1",
                    Title = "Title 1",
                    Url = "Url 1",
                    Status = SiteStatus.Active
                },
                new SiteDbEntity
                {
                    Id = _siteId2,
                    Name = "Name 2",
                    Title = "Title 2",
                    Url = "Url 2",
                    Status = SiteStatus.Active
                },
                new SiteDbEntity
                {
                    Status = SiteStatus.Deleted
                }
            );

            _dbContext.SaveChanges();

            var autoMapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
            });

            _sut = new SiteRepository(_dbContext, autoMapperConfig.CreateMapper());
        }

        [Test]
        public void Should_return_site_by_id()
        {
            var actual = _sut.GetById(_siteId1);
            Assert.NotNull(actual);
        }

        [Test]
        public void Should_return_site_by_name()
        {
            var actual = _sut.GetByName("Name 1");
            Assert.NotNull(actual);
        }

        [Test]
        public void Should_return_site_by_url()
        {
            var actual = _sut.GetByUrl("Url 1");
            Assert.NotNull(actual);
        }

        [Test]
        public void Should_return_all_sites()
        {
            var actual = _sut.GetAll();
            Assert.AreEqual(2, actual.Count);
        }

        [Test]
        public void Should_save_new_site()
        {
            var newSite = SiteFactory.Site(Guid.NewGuid(), "Name 3");
            var newSiteDbEntity = new SiteDbEntity
            {
                Id = newSite.Id,
                Name = newSite.Name
            };

            _sut.Create(newSite);

            var actual = _sut.GetById(newSite.Id);

            Assert.NotNull(actual);
        }

        [Test]
        public void Should_update_site()
        {
            var newSiteName = "New Title 1";

            var siteToUpdate = SiteFactory.Site(_siteId1, newSiteName);

            _sut.Update(siteToUpdate);

            var updatedSite = _sut.GetById(_siteId1);

            Assert.AreEqual(newSiteName, updatedSite.Title);
        }
    }
}
