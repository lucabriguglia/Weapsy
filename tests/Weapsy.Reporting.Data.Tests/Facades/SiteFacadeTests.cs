using System;
using Moq;
using NUnit.Framework;
using Weapsy.Domain.Languages;
using Weapsy.Domain.Sites;
using Weapsy.Infrastructure.Caching;
using Weapsy.Reporting.Data.Sites;
using Weapsy.Reporting.Pages;
using Weapsy.Reporting.Sites;

namespace Weapsy.Reporting.Data.Tests.Facades
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
            var pageFacadeMock = new Mock<IPageFacade>();
            var cacheManagerMock = new Mock<ICacheManager>();

            var mapperMock = new Mock<AutoMapper.IMapper>();
            mapperMock.Setup(x => x.Map<SiteAdminModel>(It.IsAny<Site>())).Returns(new SiteAdminModel());

            _sut = new SiteFacade(siteRepositoryMock.Object, 
                languageRepositoryMock.Object, 
                pageFacadeMock.Object, 
                cacheManagerMock.Object, 
                mapperMock.Object);
        }
    }
}
