using Moq;
using NUnit.Framework;
using System;
using Weapsy.Core.Caching;
using Weapsy.Domain.Languages;
using Weapsy.Domain.Sites;
using Weapsy.Reporting.Data.Default.Sites;
using Weapsy.Reporting.Sites;

namespace Weapsy.Reporting.Data.Default.Tests
{
    [TestFixture]
    public class SiteFacadeTests
    {
        private ISiteFacade _sut;
        private Guid _siteId;

        [SetUp]
        public void Setup()
        {
            _siteId = Guid.NewGuid();

            var siteRepositoryMock = new Mock<ISiteRepository>();
            var languageRepositoryMock = new Mock<ILanguageRepository>();
            var cacheManagerMock = new Mock<ICacheManager>();

            var mapperMock = new Mock<AutoMapper.IMapper>();
            mapperMock.Setup(x => x.Map<SiteAdminModel>(It.IsAny<Site>())).Returns(new SiteAdminModel());

            _sut = new SiteFacade(siteRepositoryMock.Object, languageRepositoryMock.Object, cacheManagerMock.Object, mapperMock.Object);
        }
    }
}
