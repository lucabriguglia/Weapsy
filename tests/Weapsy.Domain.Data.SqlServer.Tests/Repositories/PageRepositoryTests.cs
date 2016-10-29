//using System;
//using System.Collections.Generic;
//using System.Linq;
//using AutoMapper;
//using Microsoft.EntityFrameworkCore;
//using NUnit.Framework;
//using Weapsy.Domain.Data.SqlServer.Repositories;
//using Weapsy.Domain.Pages;
//using Weapsy.Tests.Factories;
//using PageDbEntity = Weapsy.Domain.Data.SqlServer.Entities.Page;
//using PageModuleDbEntity = Weapsy.Domain.Data.SqlServer.Entities.PageModule;
//using PageModuleLocalisationDbEntity = Weapsy.Domain.Data.SqlServer.Entities.PageModuleLocalisation;

//namespace Weapsy.Domain.Data.SqlServer.Tests.Repositories
//{
//    [TestFixture]
//    public class PageRepositoryTests
//    {
//        private DbContextOptions<WeapsyDbContext> _contextOptions;
//        private Guid _siteId;
//        private Guid _pageId1;
//        private Guid _pageId2;
//        private Guid _moduleId1;
//        private Guid _pageModuleId1;
//        private Guid _languageId1;

//        [SetUp]
//        public void SetUp()
//        {
//            _contextOptions = Shared.CreateContextOptions();

//            using (var context = new WeapsyDbContext(_contextOptions))
//            {
//                _siteId = Guid.NewGuid();
//                _pageId1 = Guid.NewGuid();
//                _pageId2 = Guid.NewGuid();
//                _moduleId1 = Guid.NewGuid();
//                _pageModuleId1 = Guid.NewGuid();
//                _languageId1 = Guid.NewGuid();

//                context.Set<PageDbEntity>().AddRange(
//                    new PageDbEntity
//                    {
//                        SiteId = _siteId,
//                        Id = _pageId1,
//                        Name = "Name 1",
//                        Url = "Url 1",
//                        Status = PageStatus.Active,
//                        PageModules = new List<PageModuleDbEntity>
//                        {
//                            new PageModuleDbEntity
//                            {
//                                PageId = _pageId1,
//                                ModuleId = _moduleId1,
//                                Id = _pageModuleId1,
//                                Title = "Title 1",
//                                Status = PageModuleStatus.Active,
//                                PageModuleLocalisations = new List<PageModuleLocalisationDbEntity>
//                                {
//                                    new PageModuleLocalisationDbEntity
//                                    {
//                                        PageModuleId = _pageModuleId1,
//                                        LanguageId = _languageId1
//                                    },
//                                    new PageModuleLocalisationDbEntity
//                                    {
//                                        PageModuleId = _pageModuleId1,
//                                        LanguageId = Guid.NewGuid()
//                                    }
//                                }
//                            },
//                            new PageModuleDbEntity
//                            {
//                                PageId = _pageId1,
//                                ModuleId = Guid.NewGuid(),
//                                Id = Guid.NewGuid(),
//                                Title = "Title 2",
//                                Status = PageModuleStatus.Deleted
//                            }
//                        }
//                    },
//                    new PageDbEntity
//                    {
//                        SiteId = _siteId,
//                        Id = _pageId2,
//                        Name = "Name 2",
//                        Url = "Url 2",
//                        Status = PageStatus.Active
//                    },
//                    new PageDbEntity
//                    {
//                        Status = PageStatus.Deleted
//                    }
//                );

//                context.SaveChanges();
//            }
//        }

//        [Test]
//        public void Should_return_page_by_id_only()
//        {
//            var actual = _sut.GetById(_pageId1);
//            Assert.NotNull(actual);
//        }

//        [Test]
//        public void Should_return_page_by_id_only_with_no_deleted_page_modules()
//        {
//            var actual = _sut.GetById(_pageId1);
//            Assert.AreEqual(0, actual.PageModules.Where(x => x.Status == PageModuleStatus.Deleted).Count());
//        }

//        [Test]
//        public void Should_return_page_by_id()
//        {
//            var actual = _sut.GetById(_siteId, _pageId1);
//            Assert.NotNull(actual);
//        }

//        [Test]
//        public void Should_return_page_by_id_with_no_deleted_page_modules()
//        {
//            var actual = _sut.GetById(_siteId, _pageId1);
//            Assert.AreEqual(0, actual.PageModules.Where(x => x.Status == PageModuleStatus.Deleted).Count());
//        }

//        [Test]
//        public void Should_return_page_by_name()
//        {
//            var actual = _sut.GetByName(_siteId, "Name 1");
//            Assert.NotNull(actual);
//        }

//        [Test]
//        public void Should_return_page_by_name_with_no_deleted_page_modules()
//        {
//            var actual = _sut.GetByName(_siteId, "Name 1");
//            Assert.AreEqual(0, actual.PageModules.Where(x => x.Status == PageModuleStatus.Deleted).Count());
//        }

//        [Test]
//        public void Should_return_page_by_url()
//        {
//            var actual = _sut.GetByUrl(_siteId, "Url 1");
//            Assert.NotNull(actual);
//        }

//        [Test]
//        public void Should_return_page_by_url_with_no_deleted_page_modules()
//        {
//            var actual = _sut.GetByUrl(_siteId, "Url 1");
//            Assert.AreEqual(0, actual.PageModules.Where(x => x.Status == PageModuleStatus.Deleted).Count());
//        }

//        [Test]
//        public void Should_return_all_pages()
//        {
//            var actual = _sut.GetAll(_siteId);
//            Assert.AreEqual(2, actual.Count);
//        }

//        [Test]
//        public void Should_return_all_pages_with_no_deleted_page_modules()
//        {
//            var actual = _sut.GetAll(_siteId);
//            foreach (var item in actual)
//                Assert.AreEqual(0, item.PageModules.Where(x => x.Status == PageModuleStatus.Deleted).Count());
//        }

//        [Test]
//        public void Should_save_new_page()
//        {
//            var newPage = PageFactory.Page(_siteId, Guid.NewGuid(), "Name 3");

//            _sut.Create(newPage);

//            var actual = _sut.GetById(newPage.Id);

//            Assert.NotNull(actual);
//        }

//        [Test]
//        public void Should_update_page()
//        {
//            var newPageName = "New Name 1";

//            var pageToUpdate = PageFactory.Page(_siteId, Guid.NewGuid(), newPageName);

//            _sut.Update(pageToUpdate);

//            var updatedPage = _sut.GetById(_pageId1);

//            Assert.AreEqual(newPageName, updatedPage.Name);
//        }
//    }
//}
