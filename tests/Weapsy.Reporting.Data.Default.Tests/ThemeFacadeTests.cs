using Moq;
using NUnit.Framework;
using System;
using Weapsy.Infrastructure.Caching;
using Weapsy.Domain.Themes;
using Weapsy.Reporting.Data.Default.Themes;
using Weapsy.Reporting.Themes;

namespace Weapsy.Reporting.Data.Default.Tests
{
    [TestFixture]
    public class ThemeFacadeTests
    {
        private IThemeFacade _sut;
        private Guid _themeId;

        [SetUp]
        public void Setup()
        {
            _themeId = Guid.NewGuid();

            var themeRepositoryMock = new Mock<IThemeRepository>();
            var cacheManagerMock = new Mock<ICacheManager>();

            var mapperMock = new Mock<AutoMapper.IMapper>();
            mapperMock.Setup(x => x.Map<ThemeAdminModel>(It.IsAny<Theme>())).Returns(new ThemeAdminModel());

            _sut = new ThemeFacade(themeRepositoryMock.Object, cacheManagerMock.Object, mapperMock.Object);
        }
    }
}
