using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using Weapsy.Core.Caching;
using Weapsy.Domain.Languages;
using Weapsy.Reporting.Data.Default.Languages;
using Weapsy.Reporting.Languages;

namespace Weapsy.Reporting.Data.Default.Tests
{
    [TestFixture]
    public class LanguageFacadeTests
    {
        private ILanguageFacade _sut;
        private Guid _siteId;
        private Guid _languageId;

        [SetUp]
        public void Setup()
        {
            _siteId = Guid.NewGuid();
            _languageId = Guid.NewGuid();

            var repositoryMock = new Mock<ILanguageRepository>();
            repositoryMock.Setup(x => x.GetById(_siteId, _languageId)).Returns(new Language());
            repositoryMock.Setup(x => x.GetAll(_siteId)).Returns(new List<Language>() { new Language(), new Language() });

            var cacheManagerMock = new Mock<ICacheManager>();

            var mapperMock = new Mock<AutoMapper.IMapper>();
            mapperMock.Setup(x => x.Map<LanguageAdminModel>(It.IsAny<Language>())).Returns(new LanguageAdminModel());

            _sut = new LanguageFacade(repositoryMock.Object, cacheManagerMock.Object, mapperMock.Object);
        }

        [Test]
        public void Should_return_models_for_admin()
        {
            var model = _sut.GetAllForAdminAsync(_siteId).Result;
            Assert.IsNotEmpty(model);
        }

        [Test]
        public void Should_return_model_for_admin()
        {
            var model = _sut.GetForAdminAsync(_siteId, _languageId).Result;
            Assert.NotNull(model);
        }
    }
}
