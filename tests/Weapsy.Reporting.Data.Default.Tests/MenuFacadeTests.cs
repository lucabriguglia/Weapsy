using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using Weapsy.Core.Caching;
using Weapsy.Domain.Model.Languages;
using Weapsy.Domain.Model.Menus;
using Weapsy.Domain.Model.Pages;
using Weapsy.Reporting.Data.Default.Menus;
using Weapsy.Reporting.Menus;

namespace Weapsy.Reporting.Data.Default.Tests
{
    [TestFixture]
    public class MenuFacadeTests
    {
        private IMenuFacade _sut;
        private Guid _siteId;
        private Guid _menuId;

        [SetUp]
        public void Setup()
        {
            _siteId = Guid.NewGuid();
            _menuId = Guid.NewGuid();

            var menuRepositoryMock = new Mock<IMenuRepository>();
            var pageRepositoryMock = new Mock<IPageRepository>();
            var languageRepositoryMock = new Mock<ILanguageRepository>();
            languageRepositoryMock.Setup(x => x.GetAll(_siteId)).Returns(new List<Language>() { new Language(), new Language() });
            var cacheManagerMock = new Mock<ICacheManager>();

            var mapperMock = new Mock<AutoMapper.IMapper>();
            mapperMock.Setup(x => x.Map<MenuAdminModel>(It.IsAny<Menu>())).Returns(new MenuAdminModel());

            _sut = new MenuFacade(menuRepositoryMock.Object, pageRepositoryMock.Object, languageRepositoryMock.Object, cacheManagerMock.Object, mapperMock.Object);
        }
    }
}
