using Moq;
using NUnit.Framework;
using System;
using Weapsy.Core.Caching;
using Weapsy.Domain.Model.Apps;
using Weapsy.Domain.Model.ModuleTypes;
using Weapsy.Reporting.Data.ModuleTypes;
using Weapsy.Reporting.ModuleTypes;

namespace Weapsy.Reporting.Data.Default.Tests
{
    [TestFixture]
    public class ModuleTypeFacadeTests
    {
        private IModuleTypeFacade sut;
        private Guid siteId;
        private Guid moduleTypeId;

        [SetUp]
        public void Setup()
        {
            siteId = Guid.NewGuid();
            moduleTypeId = Guid.NewGuid();

            var moduleTypeRepositoryMock = new Mock<IModuleTypeRepository>();
            var appRepositoryMock = new Mock<IAppRepository>();
            var cacheManagerMock = new Mock<ICacheManager>();

            var mapperMock = new Mock<AutoMapper.IMapper>();
            mapperMock.Setup(x => x.Map<ModuleTypeAdminModel>(It.IsAny<ModuleType>())).Returns(new ModuleTypeAdminModel());

            sut = new ModuleTypeFacade(moduleTypeRepositoryMock.Object,
                appRepositoryMock.Object,
                cacheManagerMock.Object, 
                mapperMock.Object);
        }
    }
}
