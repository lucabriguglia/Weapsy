using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Weapsy.Domain.Pages;
using Weapsy.Tests.Factories;
using PageDbEntity = Weapsy.Data.Entities.Page;
using PageLocalisation = Weapsy.Data.Entities.PageLocalisation;
using PageModuleDbEntity = Weapsy.Data.Entities.PageModule;
using PageModuleLocalisationDbEntity = Weapsy.Data.Entities.PageModuleLocalisation;
using System.Linq;
using Weapsy.Data.Domain;

namespace Weapsy.Data.Tests.Repositories
{
    [TestFixture]
    public class PageRepositoryTests
    {
        private DbContextOptions<WeapsyDbContext> _contextOptions;
        private Guid _siteId;
        private Guid _pageId1;
        private Guid _pageId2;
        private Guid _moduleId1;
        private Guid _pageModuleId1;
        private Guid _languageId1;
        private Guid _deletedPageId;

        [SetUp]
        public void SetUp()
        {
            _contextOptions = DbContextShared.CreateContextOptions();

            using (var context = new WeapsyDbContext(_contextOptions))
            {
                _siteId = Guid.NewGuid();
                _pageId1 = Guid.NewGuid();
                _pageId2 = Guid.NewGuid();
                _moduleId1 = Guid.NewGuid();
                _pageModuleId1 = Guid.NewGuid();
                _languageId1 = Guid.NewGuid();
                _deletedPageId = Guid.NewGuid();

                context.Pages.AddRange(
                    new PageDbEntity
                    {
                        SiteId = _siteId,
                        Id = _pageId1,
                        Name = "Name 1",
                        Url = "Url 1",
                        Status = PageStatus.Active,
                        PageModules = new List<PageModuleDbEntity>
                        {
                            new PageModuleDbEntity
                            {
                                PageId = _pageId1,
                                ModuleId = _moduleId1,
                                Id = _pageModuleId1,
                                Title = "Title 1",
                                Status = PageModuleStatus.Active,
                                PageModuleLocalisations = new List<PageModuleLocalisationDbEntity>
                                {
                                    new PageModuleLocalisationDbEntity
                                    {
                                        PageModuleId = _pageModuleId1,
                                        LanguageId = _languageId1
                                    },
                                    new PageModuleLocalisationDbEntity
                                    {
                                        PageModuleId = _pageModuleId1,
                                        LanguageId = Guid.NewGuid()
                                    }
                                }
                            },
                            new PageModuleDbEntity
                            {
                                PageId = _pageId1,
                                ModuleId = Guid.NewGuid(),
                                Id = Guid.NewGuid(),
                                Title = "Title 2",
                                Status = PageModuleStatus.Deleted
                            }
                        },
                        PageLocalisations = new List<PageLocalisation>
                        {
                            new PageLocalisation
                            {
                                PageId = _pageId1,
                                LanguageId = _languageId1,
                                Url = "localised-url-1"
                            }
                        }
                    },
                    new PageDbEntity
                    {
                        SiteId = _siteId,
                        Id = _pageId2,
                        Name = "Name 2",
                        Url = "Url 2",
                        Status = PageStatus.Active
                    },
                    new PageDbEntity
                    {
                        Id = _deletedPageId,
                        Status = PageStatus.Deleted
                    }
                );

                context.SaveChanges();
            }
        }

        [Test]
        public void Should_return_null_if_page_is_deleted()
        {
            using (var context = new WeapsyDbContext(_contextOptions))
            {
                var repository = new PageRepository(DbContextShared.CreateNewContextFactory(context), Shared.CreateNewMapper());
                var page = repository.GetById(_deletedPageId);

                Assert.Null(page);
            }
        }

        [Test]
        public void Should_return_page_by_id_only()
        {
            using (var context = new WeapsyDbContext(_contextOptions))
            {
                var repository = new PageRepository(DbContextShared.CreateNewContextFactory(context), Shared.CreateNewMapper());
                var page = repository.GetById(_pageId1);

                Assert.NotNull(page);
            }
        }

        [Test]
        public void Should_return_page_by_id_only_with_no_deleted_page_modules()
        {
            using (var context = new WeapsyDbContext(_contextOptions))
            {
                var repository = new PageRepository(DbContextShared.CreateNewContextFactory(context), Shared.CreateNewMapper());
                var page = repository.GetById(_pageId1);

                Assert.AreEqual(0, page.PageModules.Count(x => x.Status == PageModuleStatus.Deleted));
            }
        }

        [Test]
        public void Should_return_page_by_id()
        {
            using (var context = new WeapsyDbContext(_contextOptions))
            {
                var repository = new PageRepository(DbContextShared.CreateNewContextFactory(context), Shared.CreateNewMapper());
                var page = repository.GetById(_siteId, _pageId1);

                Assert.NotNull(page);
            }
        }

        [Test]
        public void Should_return_page_by_id_with_no_deleted_page_modules()
        {
            using (var context = new WeapsyDbContext(_contextOptions))
            {
                var repository = new PageRepository(DbContextShared.CreateNewContextFactory(context), Shared.CreateNewMapper());
                var page = repository.GetById(_siteId, _pageId1);

                Assert.AreEqual(0, page.PageModules.Count(x => x.Status == PageModuleStatus.Deleted));
            }
        }

        [Test]
        public void Should_return_page_id_by_name()
        {
            using (var context = new WeapsyDbContext(_contextOptions))
            {
                var repository = new PageRepository(DbContextShared.CreateNewContextFactory(context), Shared.CreateNewMapper());
                var pageId = repository.GetPageIdByName(_siteId, "Name 2");

                Assert.AreEqual(_pageId2, pageId);
            }
        }

        [Test]
        public void Should_return_page_id_by_slug()
        {
            using (var context = new WeapsyDbContext(_contextOptions))
            {
                var repository = new PageRepository(DbContextShared.CreateNewContextFactory(context), Shared.CreateNewMapper());
                var pageId = repository.GetPageIdBySlug(_siteId, "Url 2");

                Assert.AreNotEqual(Guid.Empty, pageId);
            }
        }

        [Test]
        public void Should_return_page_id_by_localised_slug()
        {
            using (var context = new WeapsyDbContext(_contextOptions))
            {
                var repository = new PageRepository(DbContextShared.CreateNewContextFactory(context), Shared.CreateNewMapper());
                var pageId = repository.GetPageIdByLocalisedSlug(_siteId, "localised-url-1");

                Assert.AreNotEqual(Guid.Empty, pageId);
            }
        }

        [Test]
        public void Should_save_new_page()
        {
            var newPage = PageFactory.Page(_siteId, Guid.NewGuid(), "Name 3");

            using (var context = new WeapsyDbContext(_contextOptions))
            {
                var repository = new PageRepository(DbContextShared.CreateNewContextFactory(context), Shared.CreateNewMapper());
                repository.Create(newPage);
            }

            using (var context = new WeapsyDbContext(_contextOptions))
            {
                var repository = new PageRepository(DbContextShared.CreateNewContextFactory(context), Shared.CreateNewMapper());
                var page = repository.GetById(_siteId, newPage.Id);

                Assert.NotNull(page);
            }
        }

        [Test]
        public void Should_update_page()
        {
            const string newPageName = "New Name 1";

            var pageToUpdate = PageFactory.Page(_siteId, _pageId1, newPageName);

            using (var context = new WeapsyDbContext(_contextOptions))
            {
                var repository = new PageRepository(DbContextShared.CreateNewContextFactory(context), Shared.CreateNewMapper());
                repository.Update(pageToUpdate);
            }

            using (var context = new WeapsyDbContext(_contextOptions))
            {
                var repository = new PageRepository(DbContextShared.CreateNewContextFactory(context), Shared.CreateNewMapper());
                var updatedPage = repository.GetById(_siteId, _pageId1);

                Assert.AreEqual(newPageName, updatedPage.Name);
            }
        }
    }
}
