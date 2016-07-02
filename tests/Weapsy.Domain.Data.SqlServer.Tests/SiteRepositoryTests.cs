using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using Weapsy.Domain.Data.SqlServer.Repositories;
using Weapsy.Domain.Model.Sites;
using Weapsy.Tests.Factories;
using SiteDbEntity = Weapsy.Domain.Data.SqlServer.Entities.Site;

namespace Weapsy.Domain.Data.SqlServer.Tests
{
    [TestFixture]
    public class SiteRepositoryTests
    {
        private ISiteRepository sut;
        private Guid siteId1;
        private Guid siteId2;

        [SetUp]
        public void SetUp()
        {
            var optionsBuilder = new DbContextOptionsBuilder<WeapsyDbContext>();
            optionsBuilder.UseInMemoryDatabase();
            var dbContext = new WeapsyDbContext(optionsBuilder.Options);

            siteId1 = Guid.NewGuid();
            siteId2 = Guid.NewGuid();

            dbContext.Set<SiteDbEntity>().AddRange(
                new SiteDbEntity
                {
                    Id = siteId1,
                    Name = "Name 1",
                    Title = "Title 1",
                    Url = "Url 1",
                    Status = SiteStatus.Active
                },
                new SiteDbEntity
                {
                    Id = siteId2,
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

            dbContext.SaveChanges();

            var mapperMock = new Moq.Mock<AutoMapper.IMapper>();
            mapperMock.Setup(x => x.Map<SiteDbEntity>(Moq.It.IsAny<Site>())).Returns(new SiteDbEntity());
            mapperMock.Setup(x => x.Map<Site>(Moq.It.IsAny<SiteDbEntity>())).Returns(new Site());

            sut = new SiteRepository(dbContext, mapperMock.Object);
        }

        [Test]
        public void Should_return_site_by_id()
        {
            var actual = sut.GetById(siteId1);
            Assert.NotNull(actual);
        }

        [Test]
        public void Should_return_site_by_name()
        {
            var actual = sut.GetByName("Name 1");
            Assert.NotNull(actual);
        }

        [Test]
        public void Should_return_site_by_url()
        {
            var actual = sut.GetByUrl("Url 1");
            Assert.NotNull(actual);
        }

        [Test]
        public void Should_return_all_sites()
        {
            var actual = sut.GetAll();
            Assert.AreEqual(2, actual.Count);
        }

        [Test]
        public void Should_save_new_site()
        {
            var newSite = SiteFactory.Site(Guid.NewGuid(), "Name 3");

            sut.Create(newSite);

            var actual = sut.GetById(newSite.Id);

            Assert.NotNull(actual);
        }

        [Test]
        public void Should_update_site()
        {
            var newSiteName = "New Title 1";

            var siteToUpdate = SiteFactory.Site(siteId1, newSiteName);

            sut.Update(siteToUpdate);

            var updatedSite = sut.GetById(siteId1);

            Assert.AreEqual(newSiteName, updatedSite.Title);
        }
    }
}
