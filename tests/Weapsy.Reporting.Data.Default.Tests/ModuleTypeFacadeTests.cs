using Moq;
using NUnit.Framework;
using System;
using Weapsy.Core.Caching;
using Weapsy.Domain.Model.Apps;
using Weapsy.Domain.Model.ModuleTypes;
using Weapsy.Reporting.Data.Default.ModuleTypes;
using Weapsy.Reporting.ModuleTypes;

namespace Weapsy.Reporting.Data.Default.Tests
{
    [TestFixture]
    public class ModuleTypeFacadeTests
    {
        private IModuleTypeFacade _sut;
        private Guid _siteId;
        private Guid _moduleTypeId;

        [SetUp]
        public void Setup()
        {
            _siteId = Guid.NewGuid();
            _moduleTypeId = Guid.NewGuid();

            var moduleTypeRepositoryMock = new Mock<IModuleTypeRepository>();
            var appRepositoryMock = new Mock<IAppRepository>();
            var cacheManagerMock = new Mock<ICacheManager>();

            var mapperMock = new Mock<AutoMapper.IMapper>();
            mapperMock.Setup(x => x.Map<ModuleTypeAdminModel>(It.IsAny<ModuleType>())).Returns(new ModuleTypeAdminModel());

            _sut = new ModuleTypeFacade(moduleTypeRepositoryMock.Object,
                appRepositoryMock.Object,
                cacheManagerMock.Object, 
                mapperMock.Object);
        }
    }
}
