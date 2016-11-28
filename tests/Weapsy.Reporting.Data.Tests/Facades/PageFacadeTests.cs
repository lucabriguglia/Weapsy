using System;
using Moq;
using NUnit.Framework;
using Weapsy.Domain.Languages;
using Weapsy.Domain.Modules;
using Weapsy.Domain.ModuleTypes;
using Weapsy.Domain.Pages;
using Weapsy.Infrastructure.Caching;
using Weapsy.Reporting.Data.Pages;
using Weapsy.Reporting.Pages;
using Weapsy.Services.Identity;

namespace Weapsy.Reporting.Data.Tests.Facades
{
    [TestFixture]
    public class PageFacadeTests
    {
        private IPageFacade _sut;
        private Guid _siteId;
        private Guid _pageId;

        [SetUp]
        public void Setup()
        {
            _siteId = Guid.NewGuid();
            _pageId = Guid.NewGuid();

            var pageRepositoryMock = new Mock<IPageRepository>();
            var languageRepositoryMock = new Mock<ILanguageRepository>();
            var moduleRepositoryMock = new Mock<IModuleRepository>();
            var moduleTypeRepositoryMock = new Mock<IModuleTypeRepository>();
            var cacheManagerMock = new Mock<ICacheManager>();

            var mapperMock = new Mock<AutoMapper.IMapper>();
            mapperMock.Setup(x => x.Map<PageAdminModel>(It.IsAny<Page>())).Returns(new PageAdminModel());

            var roleServiceMock = new Mock<IRoleService>();
            var pageInfoFactoryMock = new Mock<IPageInfoFactory>();
            var pageAdminFactoryMock = new Mock<IPageAdminFactory>();

            _sut = new PageFacade(pageRepositoryMock.Object, 
                languageRepositoryMock.Object,
                cacheManagerMock.Object, 
                mapperMock.Object,
                roleServiceMock.Object,
                pageInfoFactoryMock.Object,
                pageAdminFactoryMock.Object);
        }
    }
}
