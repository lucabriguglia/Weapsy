using System;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Weapsy.Data.Domain;
using Weapsy.Domain.Sites;
using Weapsy.Tests.Factories;
using SiteDbEntity = Weapsy.Data.Entities.Site;

namespace Weapsy.Data.Tests.Repositories
{
    [Ignore("To be removed")]
    [TestFixture]
    public class SiteRepositoryTests
    {
        private DbContextOptions<WeapsyDbContext> _contextOptions;
        private Guid _siteId1;
        private Guid _siteId2;
        private Guid _deletedSiteId;

        [SetUp]
        public void SetUp()
        {
            _contextOptions = DbContextShared.CreateContextOptions();

            using (var context = new WeapsyDbContext(_contextOptions))
            {
                _siteId1 = Guid.NewGuid();
                _siteId2 = Guid.NewGuid();
                _deletedSiteId = Guid.NewGuid();

                context.Sites.AddRange(
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
                        Id = _deletedSiteId,
                        Status = SiteStatus.Deleted
                    }
                );

                context.SaveChanges();
            }
        }

        [Test]
        public void Should_return_null_if_site_is_deleted()
        {
            using (var context = new WeapsyDbContext(_contextOptions))
            {
                var repository = new SiteRepository(DbContextShared.CreateNewContextFactory(context), Shared.CreateNewMapper());
                var site = repository.GetById(_deletedSiteId);

                Assert.Null(site);
            }
        }

        [Test]
        public void Should_return_site_by_id()
        {
            using (var context = new WeapsyDbContext(_contextOptions))
            {
                var repository = new SiteRepository(DbContextShared.CreateNewContextFactory(context), Shared.CreateNewMapper());
                var site = repository.GetById(_siteId1);

                Assert.NotNull(site);
            }
        }

        [Test]
        public void Should_return_site_by_name()
        {
            using (var context = new WeapsyDbContext(_contextOptions))
            {
                var repository = new SiteRepository(DbContextShared.CreateNewContextFactory(context), Shared.CreateNewMapper());
                var site = repository.GetByName("Name 1");

                Assert.NotNull(site);
            }
        }

        [Test]
        public void Should_return_site_by_url()
        {
            using (var context = new WeapsyDbContext(_contextOptions))
            {
                var repository = new SiteRepository(DbContextShared.CreateNewContextFactory(context), Shared.CreateNewMapper());
                var site = repository.GetByUrl("Url 1");

                Assert.NotNull(site);
            }
        }

        [Test]
        public void Should_save_new_site()
        {
            var newSite = SiteFactory.CreateNew(Guid.NewGuid(), "Name 3");

            using (var context = new WeapsyDbContext(_contextOptions))
            {
                var repository = new SiteRepository(DbContextShared.CreateNewContextFactory(context), Shared.CreateNewMapper());
                repository.Create(newSite);
            }

            using (var context = new WeapsyDbContext(_contextOptions))
            {
                var repository = new SiteRepository(DbContextShared.CreateNewContextFactory(context), Shared.CreateNewMapper());
                var language = repository.GetById(newSite.Id);

                Assert.NotNull(language);
            }
        }

        [Test]
        public void Should_update_site()
        {
            const string newSiteName = "New Title 1";

            var siteToUpdate = SiteFactory.CreateNew(_siteId1, newSiteName);

            using (var context = new WeapsyDbContext(_contextOptions))
            {
                var repository = new SiteRepository(DbContextShared.CreateNewContextFactory(context), Shared.CreateNewMapper());
                repository.Update(siteToUpdate);
            }

            using (var context = new WeapsyDbContext(_contextOptions))
            {
                var repository = new SiteRepository(DbContextShared.CreateNewContextFactory(context), Shared.CreateNewMapper());
                var updatedSite = repository.GetById(_siteId1);

                Assert.AreEqual(newSiteName, updatedSite.Name);
            }
        }
    }
}
