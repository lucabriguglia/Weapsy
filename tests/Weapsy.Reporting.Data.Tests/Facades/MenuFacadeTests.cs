using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Weapsy.Domain.Languages;
using Weapsy.Domain.Menus;
using Weapsy.Domain.Pages;
using Weapsy.Infrastructure.Caching;
using Weapsy.Reporting.Data.Menus;
using Weapsy.Reporting.Menus;
using Weapsy.Services.Identity;

namespace Weapsy.Reporting.Data.Tests.Facades
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

            var roleServiceMock = new Mock<IRoleService>();

            _sut = new MenuFacade(menuRepositoryMock.Object, 
                pageRepositoryMock.Object, 
                languageRepositoryMock.Object, 
                cacheManagerMock.Object, 
                mapperMock.Object,
                roleServiceMock.Object);
        }
    }
}
