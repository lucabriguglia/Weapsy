using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using Weapsy.Core.Caching;
using Weapsy.Domain.Model.Languages;
using Weapsy.Reporting.Data.Languages;
using Weapsy.Reporting.Languages;

namespace Weapsy.Reporting.Data.Default.Tests
{
    [TestFixture]
    public class LanguageFacadeTests
    {
        private ILanguageFacade sut;
        private Guid siteId;
        private Guid languageId;

        [SetUp]
        public void Setup()
        {
            siteId = Guid.NewGuid();
            languageId = Guid.NewGuid();

            var repositoryMock = new Mock<ILanguageRepository>();
            repositoryMock.Setup(x => x.GetById(siteId, languageId)).Returns(new Language());
            repositoryMock.Setup(x => x.GetAll(siteId)).Returns(new List<Language>() { new Language(), new Language() });

            var cacheManagerMock = new Mock<ICacheManager>();

            var mapperMock = new Mock<AutoMapper.IMapper>();
            mapperMock.Setup(x => x.Map<LanguageAdminModel>(It.IsAny<Language>())).Returns(new LanguageAdminModel());

            sut = new LanguageFacade(repositoryMock.Object, cacheManagerMock.Object, mapperMock.Object);
        }

        [Test]
        public void Should_return_models_for_admin()
        {
            var model = sut.GetAllForAdminAsync(siteId).Result;
            Assert.IsNotEmpty(model);
        }

        [Test]
        public void Should_return_model_for_admin()
        {
            var model = sut.GetForAdminAsync(siteId, languageId).Result;
            Assert.NotNull(model);
        }
    }
}
