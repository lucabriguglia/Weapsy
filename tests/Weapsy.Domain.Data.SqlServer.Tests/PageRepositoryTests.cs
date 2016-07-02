using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Weapsy.Domain.Data.SqlServer.Repositories;
using Weapsy.Domain.Model.Pages;
using Weapsy.Tests.Factories;
using PageDbEntity = Weapsy.Domain.Data.SqlServer.Entities.Page;
using PageModuleDbEntity = Weapsy.Domain.Data.SqlServer.Entities.PageModule;
using PageModuleLocalisationDbEntity = Weapsy.Domain.Data.SqlServer.Entities.PageModuleLocalisation;

namespace Weapsy.Domain.Data.SqlServer.Tests
{
    [TestFixture]
    public class PageRepositoryTests
    {
        private IPageRepository sut;
        private Guid siteId;
        private Guid pageId1;
        private Guid pageId2;
        private Guid moduleId1;
        private Guid pageModuleId1;
        private Guid languageId1;

        [SetUp]
        public void SetUp()
        {
            var optionsBuilder = new DbContextOptionsBuilder<WeapsyDbContext>();
            optionsBuilder.UseInMemoryDatabase();
            var dbContext = new WeapsyDbContext(optionsBuilder.Options);

            siteId = Guid.NewGuid();
            pageId1 = Guid.NewGuid();
            pageId2 = Guid.NewGuid();
            moduleId1 = Guid.NewGuid();
            pageModuleId1 = Guid.NewGuid();
            languageId1 = Guid.NewGuid();

            dbContext.Set<PageDbEntity>().AddRange(
                new PageDbEntity
                {
                    SiteId = siteId,
                    Id = pageId1,
                    Name = "Name 1",
                    Url = "Url 1",
                    Status = PageStatus.Active,
                    PageModules = new List<PageModuleDbEntity>
                    {
                        new PageModuleDbEntity
                        {
                            PageId = pageId1,
                            ModuleId = moduleId1,
                            Id = pageModuleId1,
                            Title = "Title 1",
                            Status = PageModuleStatus.Active,
                            PageModuleLocalisations = new List<PageModuleLocalisationDbEntity>
                            {
                                new PageModuleLocalisationDbEntity
                                {
                                    PageModuleId = pageModuleId1,
                                    LanguageId = languageId1
                                },
                                new PageModuleLocalisationDbEntity
                                {
                                    PageModuleId = pageModuleId1,
                                    LanguageId = Guid.NewGuid()
                                }
                            }
                        },
                        new PageModuleDbEntity
                        {
                            PageId = pageId1,
                            ModuleId = Guid.NewGuid(),
                            Id = Guid.NewGuid(),
                            Title = "Title 2",
                            Status = PageModuleStatus.Deleted
                        }
                    }
                },
                new PageDbEntity
                {
                    SiteId = siteId,
                    Id = pageId2,
                    Name = "Name 2",
                    Url = "Url 2",
                    Status = PageStatus.Active
                },
                new PageDbEntity
                {
                    Status = PageStatus.Deleted
                }
            );

            dbContext.SaveChanges();

            var mapperMock = new Moq.Mock<AutoMapper.IMapper>();
            mapperMock.Setup(x => x.Map<PageDbEntity>(Moq.It.IsAny<Page>())).Returns(new PageDbEntity());
            mapperMock.Setup(x => x.Map<Page>(Moq.It.IsAny<PageDbEntity>())).Returns(new Page());

            sut = new PageRepository(dbContext, mapperMock.Object);
        }

        [Test]
        public void Should_return_page_by_id_only()
        {
            var actual = sut.GetById(pageId1);
            Assert.NotNull(actual);
        }

        [Test]
        public void Should_return_page_by_id_only_with_no_deleted_page_modules()
        {
            var actual = sut.GetById(pageId1);
            Assert.AreEqual(0, actual.PageModules.Where(x => x.Status == PageModuleStatus.Deleted).Count());
        }

        [Test]
        public void Should_return_page_by_id()
        {
            var actual = sut.GetById(siteId, pageId1);
            Assert.NotNull(actual);
        }

        [Test]
        public void Should_return_page_by_id_with_no_deleted_page_modules()
        {
            var actual = sut.GetById(siteId, pageId1);
            Assert.AreEqual(0, actual.PageModules.Where(x => x.Status == PageModuleStatus.Deleted).Count());
        }

        [Test]
        public void Should_return_page_by_name()
        {
            var actual = sut.GetByName(siteId, "Name 1");
            Assert.NotNull(actual);
        }

        [Test]
        public void Should_return_page_by_name_with_no_deleted_page_modules()
        {
            var actual = sut.GetByName(siteId, "Name 1");
            Assert.AreEqual(0, actual.PageModules.Where(x => x.Status == PageModuleStatus.Deleted).Count());
        }

        [Test]
        public void Should_return_page_by_url()
        {
            var actual = sut.GetByUrl(siteId, "Url 1");
            Assert.NotNull(actual);
        }

        [Test]
        public void Should_return_page_by_url_with_no_deleted_page_modules()
        {
            var actual = sut.GetByUrl(siteId, "Url 1");
            Assert.AreEqual(0, actual.PageModules.Where(x => x.Status == PageModuleStatus.Deleted).Count());
        }

        [Test]
        public void Should_return_all_pages()
        {
            var actual = sut.GetAll(siteId);
            Assert.AreEqual(2, actual.Count);
        }

        [Test]
        public void Should_return_all_pages_with_no_deleted_page_modules()
        {
            var actual = sut.GetAll(siteId);
            foreach (var item in actual)
                Assert.AreEqual(0, item.PageModules.Where(x => x.Status == PageModuleStatus.Deleted).Count());
        }

        [Test]
        public void Should_save_new_page()
        {
            var newPage = PageFactory.Page(siteId, Guid.NewGuid(), "Name 3");

            sut.Create(newPage);

            var actual = sut.GetById(newPage.Id);

            Assert.NotNull(actual);
        }

        [Test]
        public void Should_update_page()
        {
            var newPageName = "New Name 1";

            var pageToUpdate = PageFactory.Page(siteId, Guid.NewGuid(), newPageName);

            sut.Update(pageToUpdate);

            var updatedPage = sut.GetById(pageId1);

            Assert.AreEqual(newPageName, updatedPage.Name);
        }
    }
}
