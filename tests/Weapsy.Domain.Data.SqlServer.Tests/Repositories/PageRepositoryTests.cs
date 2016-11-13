using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Weapsy.Domain.Data.SqlServer.Repositories;
using Weapsy.Domain.Pages;
using Weapsy.Tests.Factories;
using PageDbEntity = Weapsy.Domain.Data.SqlServer.Entities.Page;
using PageModuleDbEntity = Weapsy.Domain.Data.SqlServer.Entities.PageModule;
using PageModuleLocalisationDbEntity = Weapsy.Domain.Data.SqlServer.Entities.PageModuleLocalisation;

namespace Weapsy.Domain.Data.SqlServer.Tests.Repositories
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
            _contextOptions = Shared.CreateContextOptions();

            using (var context = new WeapsyDbContext(_contextOptions))
            {
                _siteId = Guid.NewGuid();
                _pageId1 = Guid.NewGuid();
                _pageId2 = Guid.NewGuid();
                _moduleId1 = Guid.NewGuid();
                _pageModuleId1 = Guid.NewGuid();
                _languageId1 = Guid.NewGuid();
                _deletedPageId = Guid.NewGuid();

                context.Set<PageDbEntity>().AddRange(
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
                var repository = new PageRepository(Shared.CreateNewContextFactory(context), Shared.CreateNewMapper());
                var page = repository.GetById(_deletedPageId);

                Assert.Null(page);
            }
        }

        [Test]
        public void Should_return_page_by_id_only()
        {
            using (var context = new WeapsyDbContext(_contextOptions))
            {
                var repository = new PageRepository(Shared.CreateNewContextFactory(context), Shared.CreateNewMapper());
                var page = repository.GetById(_pageId1);

                Assert.NotNull(page);
            }
        }

        [Test]
        public void Should_return_page_by_id_only_with_no_deleted_page_modules()
        {
            using (var context = new WeapsyDbContext(_contextOptions))
            {
                var repository = new PageRepository(Shared.CreateNewContextFactory(context), Shared.CreateNewMapper());
                var page = repository.GetById(_pageId1);

                Assert.AreEqual(0, page.PageModules.Count(x => x.Status == PageModuleStatus.Deleted));
            }
        }

        [Test]
        public void Should_return_page_by_id()
        {
            using (var context = new WeapsyDbContext(_contextOptions))
            {
                var repository = new PageRepository(Shared.CreateNewContextFactory(context), Shared.CreateNewMapper());
                var page = repository.GetById(_siteId, _pageId1);

                Assert.NotNull(page);
            }
        }

        [Test]
        public void Should_return_page_by_id_with_no_deleted_page_modules()
        {
            using (var context = new WeapsyDbContext(_contextOptions))
            {
                var repository = new PageRepository(Shared.CreateNewContextFactory(context), Shared.CreateNewMapper());
                var page = repository.GetById(_siteId, _pageId1);

                Assert.AreEqual(0, page.PageModules.Count(x => x.Status == PageModuleStatus.Deleted));
            }
        }

        [Test]
        public void Should_return_page_by_name()
        {
            using (var context = new WeapsyDbContext(_contextOptions))
            {
                var repository = new PageRepository(Shared.CreateNewContextFactory(context), Shared.CreateNewMapper());
                var page = repository.GetByName(_siteId, "Name 1");

                Assert.NotNull(page);
            }
        }

        [Test]
        public void Should_return_page_by_name_with_no_deleted_page_modules()
        {
            using (var context = new WeapsyDbContext(_contextOptions))
            {
                var repository = new PageRepository(Shared.CreateNewContextFactory(context), Shared.CreateNewMapper());
                var page = repository.GetByName(_siteId, "Name 1");

                Assert.AreEqual(0, page.PageModules.Count(x => x.Status == PageModuleStatus.Deleted));
            }
        }

        [Test]
        public void Should_return_page_by_url()
        {
            using (var context = new WeapsyDbContext(_contextOptions))
            {
                var repository = new PageRepository(Shared.CreateNewContextFactory(context), Shared.CreateNewMapper());
                var page = repository.GetByUrl(_siteId, "Url 1");

                Assert.NotNull(page);
            }
        }

        [Test]
        public void Should_return_page_by_url_with_no_deleted_page_modules()
        {
            using (var context = new WeapsyDbContext(_contextOptions))
            {
                var repository = new PageRepository(Shared.CreateNewContextFactory(context), Shared.CreateNewMapper());
                var page = repository.GetByUrl(_siteId, "Url 1");

                Assert.AreEqual(0, page.PageModules.Count(x => x.Status == PageModuleStatus.Deleted));
            }
        }

        [Test]
        public void Should_return_all_pages()
        {
            using (var context = new WeapsyDbContext(_contextOptions))
            {
                var repository = new PageRepository(Shared.CreateNewContextFactory(context), Shared.CreateNewMapper());
                var pages = repository.GetAll(_siteId);

                Assert.AreEqual(2, pages.Count);
            }
        }

        [Test]
        public void Should_return_all_pages_with_no_deleted_page_modules()
        {
            using (var context = new WeapsyDbContext(_contextOptions))
            {
                var repository = new PageRepository(Shared.CreateNewContextFactory(context), Shared.CreateNewMapper());
                var pages = repository.GetAll(_siteId);

                foreach (var page in pages)
                    Assert.AreEqual(0, page.PageModules.Count(x => x.Status == PageModuleStatus.Deleted));
            }
        }

        [Test]
        public void Should_save_new_page()
        {
            var newPage = PageFactory.Page(_siteId, Guid.NewGuid(), "Name 3");

            using (var context = new WeapsyDbContext(_contextOptions))
            {
                var repository = new PageRepository(Shared.CreateNewContextFactory(context), Shared.CreateNewMapper());
                repository.Create(newPage);
            }

            using (var context = new WeapsyDbContext(_contextOptions))
            {
                var repository = new PageRepository(Shared.CreateNewContextFactory(context), Shared.CreateNewMapper());
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
                var repository = new PageRepository(Shared.CreateNewContextFactory(context), Shared.CreateNewMapper());
                repository.Update(pageToUpdate);
            }

            using (var context = new WeapsyDbContext(_contextOptions))
            {
                var repository = new PageRepository(Shared.CreateNewContextFactory(context), Shared.CreateNewMapper());
                var updatedPage = repository.GetById(_siteId, _pageId1);

                Assert.AreEqual(newPageName, updatedPage.Name);
            }
        }
    }
}
