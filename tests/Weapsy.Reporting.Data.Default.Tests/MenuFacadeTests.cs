using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using Weapsy.Core.Caching;
using Weapsy.Domain.Model.Languages;
using Weapsy.Domain.Model.Menus;
using Weapsy.Domain.Model.Pages;
using Weapsy.Reporting.Data.Menus;
using Weapsy.Reporting.Menus;

namespace Weapsy.Reporting.Data.Default.Tests
{
    [TestFixture]
    public class MenuFacadeTests
    {
        private IMenuFacade sut;
        private Guid siteId;
        private Guid menuId;

        [SetUp]
        public void Setup()
        {
            siteId = Guid.NewGuid();
            menuId = Guid.NewGuid();

            var menuRepositoryMock = new Mock<IMenuRepository>();
            var pageRepositoryMock = new Mock<IPageRepository>();
            var languageRepositoryMock = new Mock<ILanguageRepository>();
            languageRepositoryMock.Setup(x => x.GetAll(siteId)).Returns(new List<Language>() { new Language(), new Language() });
            var cacheManagerMock = new Mock<ICacheManager>();

            var mapperMock = new Mock<AutoMapper.IMapper>();
            mapperMock.Setup(x => x.Map<MenuAdminModel>(It.IsAny<Menu>())).Returns(new MenuAdminModel());

            sut = new MenuFacade(menuRepositoryMock.Object, pageRepositoryMock.Object, languageRepositoryMock.Object, cacheManagerMock.Object, mapperMock.Object);
        }
    }
}
