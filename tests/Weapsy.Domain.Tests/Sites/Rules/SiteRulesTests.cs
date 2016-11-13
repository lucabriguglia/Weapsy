using System;
using Moq;
using NUnit.Framework;
using Weapsy.Domain.Sites;
using Weapsy.Domain.Sites.Rules;
using Weapsy.Tests.Factories;

namespace Weapsy.Domain.Tests.Sites.Rules
{
    [TestFixture]
    public class SiteRulesTests
    {
        [Test]
        public void Should_return_false_if_site_does_not_exists()
        {
            var id = Guid.NewGuid();

            var repositoryMock = new Mock<ISiteRepository>();
            repositoryMock.Setup(x => x.GetById(id)).Returns((Site)null);

            var sut = new SiteRules(repositoryMock.Object);

            var actual = sut.DoesSiteExist(id);

            Assert.AreEqual(false, actual);
        }

        [Test]
        public void Should_return_true_if_site_exists()
        {
            var id = Guid.NewGuid();

            var repositoryMock = new Mock<ISiteRepository>();
            repositoryMock.Setup(x => x.GetById(id)).Returns(new Site());

            var sut = new SiteRules(repositoryMock.Object);

            var actual = sut.DoesSiteExist(id);

            Assert.AreEqual(true, actual);
        }

        [Test]
        public void Should_return_false_if_site_id_is_not_unique()
        {
            var id = Guid.NewGuid();

            var repositoryMock = new Mock<ISiteRepository>();
            repositoryMock.Setup(x => x.GetById(id)).Returns(new Site());

            var sut = new SiteRules(repositoryMock.Object);

            var actual = sut.IsSiteIdUnique(id);

            Assert.AreEqual(false, actual);
        }

        [Test]
        public void Should_return_true_if_site_id_is_unique()
        {
            var id = Guid.NewGuid();

            var repositoryMock = new Mock<ISiteRepository>();
            repositoryMock.Setup(x => x.GetById(id)).Returns((Site)null);

            var sut = new SiteRules(repositoryMock.Object);

            var actual = sut.IsSiteIdUnique(id);

            Assert.AreEqual(true, actual);
        }

        [TestCase("a@b")]
        [TestCase("a!b")]
        [TestCase("")]
        public void Should_return_false_if_site_name_is_not_valid(string name)
        {
            var sut = new SiteRules(new Mock<ISiteRepository>().Object);
            var actual = sut.IsSiteNameValid(name);
            Assert.AreEqual(false, actual);
        }

        [TestCase("ab")]
        [TestCase("a-b")]
        [TestCase("a_b")]
        [TestCase("a1-b2")]
        [TestCase("a b")]
        public void Should_return_true_if_site_name_is_valid(string name)
        {
            var sut = new SiteRules(new Mock<ISiteRepository>().Object);
            var actual = sut.IsSiteNameValid(name);
            Assert.AreEqual(true, actual);
        }

        [Test]
        public void Should_return_false_if_site_name_is_not_unique()
        {
            var name = "My Site";

            var repositoryMock = new Mock<ISiteRepository>();
            repositoryMock.Setup(x => x.GetByName(name)).Returns(new Site());

            var sut = new SiteRules(repositoryMock.Object);

            var actual = sut.IsSiteNameUnique(name);

            Assert.AreEqual(false, actual);
        }

        [Test]
        public void Should_return_true_if_site_name_is_unique()
        {
            var name = "My Site";

            var repositoryMock = new Mock<ISiteRepository>();
            repositoryMock.Setup(x => x.GetByName(name)).Returns((Site)null);

            var sut = new SiteRules(repositoryMock.Object);

            var actual = sut.IsSiteNameUnique(name);

            Assert.AreEqual(true, actual);
        }

        [TestCase("a@b")]
        [TestCase("a!b")]
        [TestCase("a b")]
        [TestCase("")]
        public void Should_return_false_if_site_url_is_not_valid(string url)
        {
            var sut = new SiteRules(new Mock<ISiteRepository>().Object);
            var actual = sut.IsSiteUrlValid(url);
            Assert.AreEqual(false, actual);
        }

        [TestCase("ab")]
        [TestCase("a-b")]
        [TestCase("a_b")]
        [TestCase("a1-b2")]
        public void Should_return_true_if_site_url_is_valid(string url)
        {
            var sut = new SiteRules(new Mock<ISiteRepository>().Object);
            var actual = sut.IsSiteUrlValid(url);
            Assert.AreEqual(true, actual);
        }

        [Test]
        public void Should_return_false_if_site_url_is_not_unique()
        {
            var url = "my-site";

            var repositoryMock = new Mock<ISiteRepository>();
            repositoryMock.Setup(x => x.GetByUrl(url)).Returns(new Site());

            var sut = new SiteRules(repositoryMock.Object);

            var actual = sut.IsSiteUrlUnique(url);

            Assert.AreEqual(false, actual);
        }

        [Test]
        public void Should_return_true_if_site_url_is_unique()
        {
            var url = "my-site";

            var repositoryMock = new Mock<ISiteRepository>();
            repositoryMock.Setup(x => x.GetByUrl(url)).Returns((Site)null);

            var sut = new SiteRules(repositoryMock.Object);

            var actual = sut.IsSiteUrlUnique(url);

            Assert.AreEqual(true, actual);
        }

        [Test]
        public void Should_return_false_if_site_url_is_not_unique_for_existing_sites()
        {
            var siteId = Guid.NewGuid();
            var url = "my-site";

            var repositoryMock = new Mock<ISiteRepository>();
            repositoryMock.Setup(x => x.GetByUrl(url)).Returns(new Site());

            var sut = new SiteRules(repositoryMock.Object);

            var actual = sut.IsSiteUrlUnique(url, siteId);

            Assert.AreEqual(false, actual);
        }

        [Test]
        public void Should_return_true_if_site_url_is_unique_for_existing_sites()
        {
            var siteId = Guid.NewGuid();
            var url = "my-site";

            var repositoryMock = new Mock<ISiteRepository>();
            repositoryMock.Setup(x => x.GetByUrl(url)).Returns((Site)null);

            var sut = new SiteRules(repositoryMock.Object);

            var actual = sut.IsSiteUrlUnique(url, siteId);

            Assert.AreEqual(true, actual);
        }

        [Test]
        public void Should_return_true_if_page_is_set_as_home_page()
        {
            var siteId = Guid.NewGuid();
            var pageId = Guid.NewGuid();

            var site = SiteFactory.CreateNew();
            site.Update(pageId);

            var repositoryMock = new Mock<ISiteRepository>();
            repositoryMock.Setup(x => x.GetById(siteId)).Returns(site);

            var sut = new SiteRules(repositoryMock.Object);

            var actual = sut.IsPageSetAsHomePage(siteId, pageId);

            Assert.AreEqual(true, actual);
        }

        [Test]
        public void Should_return_false_if_page_is_not_set_as_home_page()
        {
            var siteId = Guid.NewGuid();
            var pageId = Guid.NewGuid();

            var repositoryMock = new Mock<ISiteRepository>();
            repositoryMock.Setup(x => x.GetById(siteId)).Returns(new Site());

            var sut = new SiteRules(repositoryMock.Object);

            var actual = sut.IsPageSetAsHomePage(siteId, pageId);

            Assert.AreEqual(false, actual);
        }
    }
}
