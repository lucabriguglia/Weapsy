using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Weapsy.Domain.Languages;
using Weapsy.Infrastructure.Caching;
using Weapsy.Reporting.Data.Default.Languages;
using Weapsy.Reporting.Languages;
using Weapsy.Tests.Factories;

namespace Weapsy.Reporting.Data.Default.Tests.Facades
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

            var language = LanguageFactory.Language(_siteId, _languageId, "", "", "");

            var repositoryMock = new Mock<ILanguageRepository>();
            repositoryMock.Setup(x => x.GetById(_siteId, _languageId)).Returns(language);
            repositoryMock.Setup(x => x.GetAll(_siteId)).Returns(new List<Language> { language });

            var cacheManagerMock = new Mock<ICacheManager>();

            _sut = new LanguageFacade(repositoryMock.Object, cacheManagerMock.Object, Shared.CreateNewMapper());
        }

        [Test]
        public void Should_return_all_models_for_admin()
        {
            var models = _sut.GetAllForAdmin(_siteId);
            Assert.IsNotEmpty(models);
        }

        [Test]
        public void Should_return_model_for_admin()
        {
            var model = _sut.GetForAdmin(_siteId, _languageId);
            Assert.NotNull(model);
        }
    }
}
