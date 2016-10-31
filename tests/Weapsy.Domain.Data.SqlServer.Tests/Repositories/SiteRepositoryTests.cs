using System;
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
        private DbContextOptions<WeapsyDbContext> _contextOptions;
        private Guid _siteId1;
        private Guid _siteId2;

        [SetUp]
        public void SetUp()
        {
            _contextOptions = Shared.CreateContextOptions();

            using (var context = new WeapsyDbContext(_contextOptions))
            {
                _siteId1 = Guid.NewGuid();
                _siteId2 = Guid.NewGuid();

                context.Set<SiteDbEntity>().AddRange(
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

                context.SaveChanges();
            }
        }

        [Test]
        public void Should_return_site_by_id()
        {
            using (var context = new WeapsyDbContext(_contextOptions))
            {
                var repository = new SiteRepository(Shared.CreateNewContextFactory(context), Shared.CreateNewMapper());
                var site = repository.GetById(_siteId1);

                Assert.NotNull(site);
            }
        }

        [Test]
        public void Should_return_site_by_name()
        {
            using (var context = new WeapsyDbContext(_contextOptions))
            {
                var repository = new SiteRepository(Shared.CreateNewContextFactory(context), Shared.CreateNewMapper());
                var site = repository.GetByName("Name 1");

                Assert.NotNull(site);
            }
        }

        [Test]
        public void Should_return_site_by_url()
        {
            using (var context = new WeapsyDbContext(_contextOptions))
            {
                var repository = new SiteRepository(Shared.CreateNewContextFactory(context), Shared.CreateNewMapper());
                var site = repository.GetByUrl("Url 1");

                Assert.NotNull(site);
            }
        }

        [Test]
        public void Should_return_all_sites()
        {
            using (var context = new WeapsyDbContext(_contextOptions))
            {
                var repository = new SiteRepository(Shared.CreateNewContextFactory(context), Shared.CreateNewMapper());
                var sites = repository.GetAll();

                Assert.AreEqual(2, sites.Count);
            }
        }

        [Test]
        public void Should_save_new_site()
        {
            var newSite = SiteFactory.Site(Guid.NewGuid(), "Name 3");

            using (var context = new WeapsyDbContext(_contextOptions))
            {
                var repository = new SiteRepository(Shared.CreateNewContextFactory(context), Shared.CreateNewMapper());
                repository.Create(newSite);
            }

            using (var context = new WeapsyDbContext(_contextOptions))
            {
                var repository = new SiteRepository(Shared.CreateNewContextFactory(context), Shared.CreateNewMapper());
                var language = repository.GetById(newSite.Id);

                Assert.NotNull(language);
            }
        }

        [Test]
        public void Should_update_site()
        {
            const string newSiteName = "New Title 1";

            var siteToUpdate = SiteFactory.Site(_siteId1, newSiteName);

            using (var context = new WeapsyDbContext(_contextOptions))
            {
                var repository = new SiteRepository(Shared.CreateNewContextFactory(context), Shared.CreateNewMapper());
                repository.Update(siteToUpdate);
            }

            using (var context = new WeapsyDbContext(_contextOptions))
            {
                var repository = new SiteRepository(Shared.CreateNewContextFactory(context), Shared.CreateNewMapper());
                var updatedSite = repository.GetById(_siteId1);

                Assert.AreEqual(newSiteName, updatedSite.Name);
            }
        }
    }
}
