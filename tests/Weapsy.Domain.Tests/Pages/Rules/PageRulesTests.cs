using System;
using Moq;
using NUnit.Framework;
using Weapsy.Domain.Pages;
using Weapsy.Domain.Pages.Rules;

namespace Weapsy.Domain.Tests.Pages.Rules
{
    [TestFixture]
    public class PageRulesTests
    {
        [Test]
        public void Should_return_false_if_page_does_not_exists()
        {
            var id = Guid.NewGuid();

            var repositoryMock = new Mock<IPageRepository>();
            repositoryMock.Setup(x => x.GetById(id)).Returns((Page)null);

            var sut = new PageRules(repositoryMock.Object);

            var actual = sut.DoesPageExist(id);

            Assert.AreEqual(false, actual);
        }

        [Test]
        public void Should_return_true_if_page_exists()
        {
            var id = Guid.NewGuid();

            var repositoryMock = new Mock<IPageRepository>();
            repositoryMock.Setup(x => x.GetById(id)).Returns(new Page());

            var sut = new PageRules(repositoryMock.Object);

            var actual = sut.DoesPageExist(id);

            Assert.AreEqual(true, actual);
        }

        [Test]
        public void Should_return_false_if_page_id_is_not_unique()
        {
            var id = Guid.NewGuid();

            var repositoryMock = new Mock<IPageRepository>();
            repositoryMock.Setup(x => x.GetById(id)).Returns(new Page());

            var sut = new PageRules(repositoryMock.Object);

            var actual = sut.IsPageIdUnique(id);

            Assert.AreEqual(false, actual);
        }

        [Test]
        public void Should_return_true_if_page_id_is_unique()
        {
            var id = Guid.NewGuid();

            var repositoryMock = new Mock<IPageRepository>();
            repositoryMock.Setup(x => x.GetById(id)).Returns((Page)null);

            var sut = new PageRules(repositoryMock.Object);

            var actual = sut.IsPageIdUnique(id);

            Assert.AreEqual(true, actual);
        }

        [TestCase("a@b")]
        [TestCase("a!b")]
        [TestCase("")]
        public void Should_return_false_if_page_name_is_not_valid(string name)
        {
            var sut = new PageRules(new Mock<IPageRepository>().Object);
            var actual = sut.IsPageNameValid(name);
            Assert.AreEqual(false, actual);
        }

        [TestCase("ab")]
        [TestCase("a-b")]
        [TestCase("a_b")]
        [TestCase("a1-b2")]
        [TestCase("a b")]
        public void Should_return_true_if_page_name_is_valid(string name)
        {
            var sut = new PageRules(new Mock<IPageRepository>().Object);
            var actual = sut.IsPageNameValid(name);
            Assert.AreEqual(true, actual);
        }

        [Test]
        public void Should_return_false_if_page_name_is_not_unique()
        {
            var siteId = Guid.NewGuid();
            var name = "My Page";

            var repositoryMock = new Mock<IPageRepository>();
            repositoryMock.Setup(x => x.GetPageIdByName(siteId, name)).Returns(Guid.NewGuid());

            var sut = new PageRules(repositoryMock.Object);

            var actual = sut.IsPageNameUnique(siteId, name);

            Assert.AreEqual(false, actual);
        }

        [Test]
        public void Should_return_true_if_page_name_is_unique()
        {
            var siteId = Guid.NewGuid();
            var name = "My Page";

            var repositoryMock = new Mock<IPageRepository>();
            repositoryMock.Setup(x => x.GetPageIdByName(siteId, name)).Returns(Guid.Empty);

            var sut = new PageRules(repositoryMock.Object);

            var actual = sut.IsPageNameUnique(siteId, name);

            Assert.AreEqual(true, actual);
        }

        [Test]
        public void Should_return_false_if_page_name_is_not_unique_for_existing_pages()
        {
            var siteId = Guid.NewGuid();
            var pageId = Guid.NewGuid();
            var name = "my-page";

            var repositoryMock = new Mock<IPageRepository>();
            repositoryMock.Setup(x => x.GetPageIdByName(siteId, name)).Returns(Guid.NewGuid());

            var sut = new PageRules(repositoryMock.Object);

            var actual = sut.IsPageNameUnique(siteId, name, pageId);

            Assert.AreEqual(false, actual);
        }

        [Test]
        public void Should_return_true_if_page_name_is_unique_for_existing_pages()
        {
            var siteId = Guid.NewGuid();
            var pageId = Guid.NewGuid();
            var name = "my-page";

            var repositoryMock = new Mock<IPageRepository>();
            repositoryMock.Setup(x => x.GetPageIdByName(siteId, name)).Returns(Guid.Empty);

            var sut = new PageRules(repositoryMock.Object);

            var actual = sut.IsPageNameUnique(siteId, name, pageId);

            Assert.AreEqual(true, actual);
        }

        [TestCase("a@b")]
        [TestCase("a!b")]
        [TestCase("a b")]
        [TestCase("a1\b2")]
        [TestCase("")]
        public void Should_return_false_if_page_url_is_not_valid(string url)
        {
            var sut = new PageRules(new Mock<IPageRepository>().Object);
            var actual = sut.IsPageUrlValid(url);
            Assert.AreEqual(false, actual);
        }

        [TestCase("ab")]
        [TestCase("a-b")]
        [TestCase("a_b")]
        [TestCase("a1-b2")]
        [TestCase("a1/b2")]
        public void Should_return_true_if_page_url_is_valid(string url)
        {
            var sut = new PageRules(new Mock<IPageRepository>().Object);
            var actual = sut.IsPageUrlValid(url);
            Assert.AreEqual(true, actual);
        }

        [TestCase("home")]
        [TestCase("one/two")]
        [TestCase("one/two/three")]
        public void Should_return_false_if_page_url_is_not_reserved(string url)
        {
            var sut = new PageRules(new Mock<IPageRepository>().Object);
            var actual = sut.IsPageUrlReserved(url);
            Assert.AreEqual(false, actual);
        }

        [TestCase("admin")]
        [TestCase("admin/pages")]
        [TestCase("profile")]
        [TestCase("profile/one/two")]
        public void Should_return_true_if_page_url_is_reserved(string url)
        {
            var sut = new PageRules(new Mock<IPageRepository>().Object);
            var actual = sut.IsPageUrlReserved(url);
            Assert.AreEqual(true, actual);
        }

        [Test]
        public void Should_return_false_if_page_url_is_not_unique()
        {
            var siteId = Guid.NewGuid();
            var url = "my-page";

            var repositoryMock = new Mock<IPageRepository>();
            repositoryMock.Setup(x => x.GetPageIdBySlug(siteId, url)).Returns(Guid.NewGuid());

            var sut = new PageRules(repositoryMock.Object);

            var actual = sut.IsSlugUnique(siteId, url);

            Assert.AreEqual(false, actual);
        }

        [Test]
        public void Should_return_true_if_page_url_is_unique()
        {
            var siteId = Guid.NewGuid();
            var url = "my-page";

            var repositoryMock = new Mock<IPageRepository>();
            repositoryMock.Setup(x => x.GetPageIdBySlug(siteId, url)).Returns(Guid.Empty);

            var sut = new PageRules(repositoryMock.Object);

            var actual = sut.IsSlugUnique(siteId, url);

            Assert.AreEqual(true, actual);
        }

        [Test]
        public void Should_return_false_if_page_url_is_not_unique_for_existing_pages()
        {
            var siteId = Guid.NewGuid();
            var pageId = Guid.NewGuid();
            var url = "my-page";

            var repositoryMock = new Mock<IPageRepository>();
            repositoryMock.Setup(x => x.GetPageIdBySlug(siteId, url)).Returns(Guid.NewGuid());

            var sut = new PageRules(repositoryMock.Object);

            var actual = sut.IsSlugUnique(siteId, url, pageId);

            Assert.AreEqual(false, actual);
        }

        [Test]
        public void Should_return_true_if_page_url_is_unique_for_existing_pages()
        {
            var siteId = Guid.NewGuid();
            var pageId = Guid.NewGuid();
            var url = "my-page";

            var repositoryMock = new Mock<IPageRepository>();
            repositoryMock.Setup(x => x.GetPageIdBySlug(siteId, url)).Returns(Guid.Empty);

            var sut = new PageRules(repositoryMock.Object);

            var actual = sut.IsSlugUnique(siteId, url, pageId);

            Assert.AreEqual(true, actual);
        }

        [Test]
        public void Should_return_false_if_localised_slug_is_not_unique()
        {
            var siteId = Guid.NewGuid();
            var slug = "my-page";

            var repositoryMock = new Mock<IPageRepository>();
            repositoryMock.Setup(x => x.GetPageIdByLocalisedSlug(siteId, slug)).Returns(Guid.NewGuid());

            var sut = new PageRules(repositoryMock.Object);

            var actual = sut.IsSlugUnique(siteId, slug);

            Assert.AreEqual(false, actual);
        }

        [Test]
        public void Should_return_true_if_localised_slug_is_unique()
        {
            var siteId = Guid.NewGuid();
            var slug = "my-page";

            var repositoryMock = new Mock<IPageRepository>();
            repositoryMock.Setup(x => x.GetPageIdByLocalisedSlug(siteId, slug)).Returns(Guid.Empty);

            var sut = new PageRules(repositoryMock.Object);

            var actual = sut.IsSlugUnique(siteId, slug);

            Assert.AreEqual(true, actual);
        }

        [Test]
        public void Should_return_false_if_localised_slug_is_not_unique_for_existing_pages()
        {
            var siteId = Guid.NewGuid();
            var slug = "my-page";

            var repositoryMock = new Mock<IPageRepository>();
            repositoryMock.Setup(x => x.GetPageIdByLocalisedSlug(siteId, slug)).Returns(Guid.NewGuid());

            var sut = new PageRules(repositoryMock.Object);

            var actual = sut.IsSlugUnique(siteId, slug, Guid.NewGuid());

            Assert.AreEqual(false, actual);
        }

        [Test]
        public void Should_return_true_if_localised_slug_is_unique_for_existing_pages()
        {
            var siteId = Guid.NewGuid();
            var slug = "my-page";

            var repositoryMock = new Mock<IPageRepository>();
            repositoryMock.Setup(x => x.GetPageIdByLocalisedSlug(siteId, slug)).Returns(Guid.Empty);

            var sut = new PageRules(repositoryMock.Object);

            var actual = sut.IsSlugUnique(siteId, slug, Guid.NewGuid());

            Assert.AreEqual(true, actual);
        }
    }
}
