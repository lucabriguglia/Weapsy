using Moq;
using NUnit.Framework;
using System;
using Weapsy.Core.Caching;
using Weapsy.Domain.Model.Languages;
using Weapsy.Domain.Model.Modules;
using Weapsy.Domain.Model.ModuleTypes;
using Weapsy.Domain.Model.Pages;
using Weapsy.Reporting.Data.Pages;
using Weapsy.Reporting.Pages;

namespace Weapsy.Reporting.Data.Default.Tests
{
    [TestFixture]
    public class PageFacadeTests
    {
        private IPageFacade sut;
        private Guid siteId;
        private Guid pageId;

        [SetUp]
        public void Setup()
        {
            siteId = Guid.NewGuid();
            pageId = Guid.NewGuid();

            var pageRepositoryMock = new Mock<IPageRepository>();
            var languageRepositoryMock = new Mock<ILanguageRepository>();
            var moduleRepositoryMock = new Mock<IModuleRepository>();
            var moduleTypeRepositoryMock = new Mock<IModuleTypeRepository>();
            var cacheManagerMock = new Mock<ICacheManager>();

            var mapperMock = new Mock<AutoMapper.IMapper>();
            mapperMock.Setup(x => x.Map<PageAdminModel>(It.IsAny<Page>())).Returns(new PageAdminModel());

            sut = new PageFacade(pageRepositoryMock.Object, 
                languageRepositoryMock.Object,
                moduleRepositoryMock.Object,
                moduleTypeRepositoryMock.Object,
                cacheManagerMock.Object, 
                mapperMock.Object);
        }
    }
}
